using Application.Commons;
using Application.Interfaces;
using Application.RequestModel.Cart;
using Application.ViewModels.Cart;
using Application.ViewModels.CartItems;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimsService _claimsService;
        public CartService(IUnitOfWork unitOfWork, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _claimsService = claimsService;
        }
        public async Task<ServiceResponse<CartViewModel>> GetCartForUserAsync()
        {
            var response = new ServiceResponse<CartViewModel>();
            try
            {
                var cart = await _unitOfWork.CartRepository.GetCartForUserAsync(_claimsService.GetCurrentUserId.Value);

                if (cart == null)
                {
                    response.Success = false;
                    response.Message = "Cart is null. Add item to cart and try again.";
                    return response;
                }

                response.Data = new CartViewModel
                {
                    Id = cart.Id,
                    UserId = cart.AccountId,
                    NumberItems = cart.Items.Count,
                    Items = cart.Items.Select(i => new CartItemViewModel
                    {
                        ProductId = i.ProductId,
                        ProductName = cart.Items.FirstOrDefault(ci => ci.ProductId == i.ProductId).Product.Name,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList(),
                    TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity)
                };
                response.Success = true;
                response.Message = "This is cart for user.";
            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceResponse<CartViewModel>> AddToCartAsync(CartRequestModel cartRequest)
        {
            var response = new ServiceResponse<CartViewModel>();

            try
            {
                var cart = await _unitOfWork.CartRepository.GetCartForUserAsync(_claimsService.GetCurrentUserId.Value);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        AccountId = _claimsService.GetCurrentUserId.Value,
                        Items = new List<CartItem>()
                    };
                    foreach (var item in cartRequest.Items)
                    {
                        var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                        if (product == null)
                        {
                            response.Success = false;
                            response.Message = $"Product with id: {item.ProductId} is not exist.";
                            return response;
                        }

                        if (item.Quantity <= 0)
                        {
                            response.Success = false;
                            response.Message = "The quantity of product must be greater than 0.";
                            return response;
                        }
                        if (product.Quantity >= item.Quantity)
                        {
                            cart.Items.Add(new CartItem
                            {
                                ProductId = item.ProductId,
                                Quantity = item.Quantity,
                                Price = product.Price
                            });
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = $"Not enough stock for product {item.ProductId}.";
                            return response;
                        }
                    }
                    await _unitOfWork.CartRepository.AddAsync(cart);
                }
                else
                {
                    foreach (var item in cartRequest.Items)
                    {
                        var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);

                        if (product == null)
                        {
                            response.Success = false;
                            response.Message = $"Product with id: {item.ProductId} is not exist.";
                            return response;
                        }

                        if (item.Quantity <= 0)
                        {
                            response.Success = false;
                            response.Message = "The quantity of product must be greater than 0.";
                            return response;
                        }

                        var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
                        if (product.Quantity >= item.Quantity)
                        {
                            if (cartItem != null)
                            {
                                cartItem.Quantity += item.Quantity;
                            }
                            else
                            {
                                cart.Items.Add(new CartItem
                                {
                                    CartId = cart.Id,
                                    ProductId = item.ProductId,
                                    Quantity = item.Quantity,
                                    Price = product.Price
                                });
                            }
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = $"Not enough stock for product {item.ProductId}.";
                            return response;
                        }
                    }
                    _unitOfWork.CartRepository.Update(cart);
                }

                await _unitOfWork.SaveChangeAsync();
                response.Data = new CartViewModel
                {
                    Id = cart.Id,
                    UserId = cart.AccountId,
                    NumberItems = cart.Items.Count,
                    Items = cart.Items.Select(i => new CartItemViewModel
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList(),
                    TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity)
                };
                response.Success = true;
                response.Message = "Add to cart successfully.";
            }

            catch (DbUpdateException ex)
            {
                response.Success = false;
                response.Message = "Failed to update data.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceResponse<CartViewModel>> RemoveFromCartAsync(CartRequestModel request)
        {
            var response = new ServiceResponse<CartViewModel>();
            try
            {
                var cart = await _unitOfWork.CartRepository.GetCartForUserAsync(_claimsService.GetCurrentUserId.Value);

                if (cart == null)
                {
                    response.Success = false;
                    response.Message = "Cart is not existed.";
                    return response;
                }

                foreach (var item in request.Items)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);

                    if (product == null)
                    {
                        response.Success = false;
                        response.Message = $"Product with id: {item.ProductId} is not exist.";
                        return response;
                    }

                    if (item.Quantity <= 0)
                    {
                        response.Success = false;
                        response.Message = "The quantity of product must be greater than 0.";
                        return response;
                    }

                    var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
                    if (cartItem != null)
                    {
                        if (item.Quantity >= cartItem.Quantity)
                        {
                            cart.Items.Remove(cartItem);
                        }
                        else
                        {
                            cartItem.Quantity -= item.Quantity;
                        }
                    }
                }
                _unitOfWork.CartRepository.Update(cart);
                await _unitOfWork.SaveChangeAsync();

                response.Data = new CartViewModel
                {
                    Id = cart.Id,
                    UserId = cart.AccountId,
                    NumberItems = cart.Items.Count,
                    Items = cart.Items.Select(i => new CartItemViewModel
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList(),
                    TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity)
                };
                response.Success = true;
                response.Message = "Removed from cart successfully.";

            }
            catch (DbUpdateException ex)
            {
                response.Success = false;
                response.Message = "Failed to update data.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteCartAsync()
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var cart = await _unitOfWork.CartRepository.GetCartForUserAsync(_claimsService.GetCurrentUserId.Value);
                if (cart == null)
                {
                    response.Success = false;
                    response.Message = "Cart is not existed";
                    return response;
                }
                _unitOfWork.CartRepository.DeleteCartItem(cart.Items);
                await _unitOfWork.SaveChangeAsync();

                _unitOfWork.CartRepository.DeleteCart(cart.Id);
                await _unitOfWork.SaveChangeAsync();

                response.Success = true;
                response.Message = "Delete cart for this user successfully.";
            }
            catch (DbUpdateException ex)
            {
                response.Success = false;
                response.Message = "Failed to update data.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }
    }
}
