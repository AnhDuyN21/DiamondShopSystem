using Application.Commons;
using Application.Interfaces;
using Application.ViewModels.CategoryDTOs;
using Application.ViewModels.DiamondDTOs;
using Application.ViewModels.ProductDTOs;
using Application.ViewModels.ProductTypeDTOS;
using AutoMapper;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IImageService _imageService;

		public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_imageService = imageService;
		}

		public async Task<ServiceResponse<IEnumerable<ProductDTO>>> GetProductsAsync()
		{
			var response = new ServiceResponse<IEnumerable<ProductDTO>>();
			try
			{
				var products = await _unitOfWork.ProductRepository.GetAllAsync();

				var productsDTO = new List<ProductDTO>();

				foreach (var product in products)
				{
					var pro = _mapper.Map<ProductDTO>(product);
					pro.Images = _unitOfWork.ImageRepository.GetImagesByProductId(pro.Id);

					var cat = await _unitOfWork.CategoryRepository.GetByIdAsync(pro.CategoryId);
					if (cat != null)
					{
						pro.Category = _mapper.Map<CategoryDTO>(cat);
					}

					var type = await _unitOfWork.ProductTypeRepository.GetByIdAsync(pro.ProductTypeId);
					if (type != null)
					{
						pro.ProductType = _mapper.Map<ProductTypeDTO>(type);
					}

					var pridia = _unitOfWork.ProductDiamondRepository.GetPrimaryDiamondsByProductId(pro.Id);
					var subdia = _unitOfWork.ProductDiamondRepository.GetSubDiamondsByProductId(pro.Id);
					pro.PrimaryDiamonds = new List<DiamondDTO>();
					foreach (var i in pridia)
					{
						var diamond = await _unitOfWork.DiamondRepository.GetByIdAsync(i);
						pro.PrimaryDiamonds.Add(_mapper.Map<DiamondDTO>(diamond));
					}
					pro.SubDiamonds = new List<DiamondDTO>();
					foreach (var i in subdia)
					{
						var diamond = await _unitOfWork.DiamondRepository.GetByIdAsync(i);
						pro.SubDiamonds.Add(_mapper.Map<DiamondDTO>(diamond));
					}
					productsDTO.Add(pro);
				}

				if (productsDTO.Count != 0)
				{
					response.Success = true;
					response.Message = "Products retrieved successfully!";
					response.Data = productsDTO;
				}
				else
				{
					response.Success = true;
					response.Message = "Not have product yet!";
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = "Error";
				response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
			}
			return response;
		}
		public async Task<ServiceResponse<ProductDTO>> GetProductByIdAsync(int id)
		{
			var response = new ServiceResponse<ProductDTO>();
			var exist = await _unitOfWork.ProductRepository.GetByIdAsync(id);
			var pro = _mapper.Map<ProductDTO>(exist);
			if (pro == null)
			{
				response.Success = false;
				response.Message = "Product is not existed!";
			}
			//else if (exist.IsDeleted == true)
			//{
			//	response.Success = false;
			//	response.Message = "Product have been deleted from the system.";
			//}
			else
			{
				response.Success = true;
				response.Message = "Product found.";
				pro.Images = _unitOfWork.ImageRepository.GetImagesByProductId(pro.Id);
				var cat = await _unitOfWork.CategoryRepository.GetByIdAsync(pro.CategoryId);
				pro.Category = _mapper.Map<CategoryDTO>(cat);
				var type = await _unitOfWork.ProductTypeRepository.GetByIdAsync(pro.ProductTypeId);
				pro.ProductType = _mapper.Map<ProductTypeDTO>(type);

				var pridia = _unitOfWork.ProductDiamondRepository.GetPrimaryDiamondsByProductId(pro.Id);
				var subdia = _unitOfWork.ProductDiamondRepository.GetSubDiamondsByProductId(pro.Id);
				pro.PrimaryDiamonds = new List<DiamondDTO>();
				foreach (var i in pridia)
				{
					var diamond = await _unitOfWork.DiamondRepository.GetByIdAsync(i);
					pro.PrimaryDiamonds.Add(_mapper.Map<DiamondDTO>(diamond));
				}
				response.Data = pro;
				pro.SubDiamonds = new List<DiamondDTO>();
				foreach (var i in subdia)
				{
					var diamond = await _unitOfWork.DiamondRepository.GetByIdAsync(i);
					pro.SubDiamonds.Add(_mapper.Map<DiamondDTO>(diamond));
				}
				response.Data = pro;
			}
			return response;
		}
		public async Task<ServiceResponse<IEnumerable<ProductDTO>>> SearchProductByNameAsync(string name)
		{
			var response = new ServiceResponse<IEnumerable<ProductDTO>>();
			try
			{
				var products = await _unitOfWork.ProductRepository.SearchProduct(name);
				var productsDTO = new List<ProductDTO>();

				foreach (var product in products)
				{
					var pro = _mapper.Map<ProductDTO>(product);
					pro.Images = _unitOfWork.ImageRepository.GetImagesByProductId(pro.Id);
					var cat = await _unitOfWork.CategoryRepository.GetByIdAsync(pro.CategoryId);
					pro.Category = _mapper.Map<CategoryDTO>(cat);
					var type = await _unitOfWork.ProductTypeRepository.GetByIdAsync(pro.ProductTypeId);
					pro.ProductType = _mapper.Map<ProductTypeDTO>(type);

					var pridia = _unitOfWork.ProductDiamondRepository.GetPrimaryDiamondsByProductId(pro.Id);
					var subdia = _unitOfWork.ProductDiamondRepository.GetSubDiamondsByProductId(pro.Id);
					pro.PrimaryDiamonds = new List<DiamondDTO>();
					foreach (var i in pridia)
					{
						var diamond = await _unitOfWork.DiamondRepository.GetByIdAsync(i);
						pro.PrimaryDiamonds.Add(_mapper.Map<DiamondDTO>(diamond));
					}
					pro.SubDiamonds = new List<DiamondDTO>();
					foreach (var i in subdia)
					{
						var diamond = await _unitOfWork.DiamondRepository.GetByIdAsync(i);
						pro.SubDiamonds.Add(_mapper.Map<DiamondDTO>(diamond));
					}
					productsDTO.Add(pro);
				}

				if (productsDTO.Count != 0)
				{
					response.Success = true;
					response.Message = "Products retrieved successfully!";
					response.Data = productsDTO;
				}
				else
				{
					response.Success = false;
					response.Message = "Not have product yet!";
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = "Error";
				response.ErrorMessages = new List<string> { ex.Message };
			}
			return response;
		}
		public async Task<ServiceResponse<CreateProductDTO>> CreateProductAsync(CreateProductDTO createProduct)
		{
			var response = new ServiceResponse<CreateProductDTO>();
			var isExisted = await _unitOfWork.ProductRepository.NameIsExisted(createProduct.Name);
			if (isExisted)
			{
				response.Success = false;
				response.Message = "Name existed!";
				return response;
			}
			try
			{
				var product = _mapper.Map<Product>(createProduct);
				product.IsDeleted = false;
				var productType = await _unitOfWork.ProductTypeRepository.GetByIdAsync(product.ProductTypeId);
				product.Price = product.Wage + productType.Price * product.Weight;
				if (!createProduct.PrimaryDiamonds.IsNullOrEmpty())
				{
					foreach (var diamond in createProduct.PrimaryDiamonds)
					{
						if (diamond != 0)
						{
							var dia = await _unitOfWork.DiamondRepository.GetByIdAsync(diamond);
							if (dia != null) dia.Quantity--;
							_unitOfWork.DiamondRepository.Update(dia);
							product.Price += dia.Price;
						}
					}
				}
				if (!createProduct.SubDiamonds.IsNullOrEmpty())
				{
					foreach (var diamond in createProduct.SubDiamonds)
					{
						if (diamond != 0)
						{
							var dia = await _unitOfWork.DiamondRepository.GetByIdAsync(diamond);
							if (dia != null) dia.Quantity--;
							_unitOfWork.DiamondRepository.Update(dia);
							product.Price += dia.Price;
						}
					}
				}
				product.Price += product.Price * 1 / 10;
				await _unitOfWork.ProductRepository.AddAsync(product);
				var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;

				if (!createProduct.PrimaryDiamonds.IsNullOrEmpty())
				{
					foreach (var diamond in createProduct.PrimaryDiamonds)
					{
						if (diamond != 0)
						{
							var productDiamond = new ProductDiamond
							{
								IsMain = true,
								DiamondId = diamond,
								ProductId = product.Id,
							};
							await _unitOfWork.ProductDiamondRepository.AddAsync(productDiamond);
							await _unitOfWork.SaveChangeAsync();
						}
					}
				}
				if (!createProduct.SubDiamonds.IsNullOrEmpty())
				{
					foreach (var diamond in createProduct.SubDiamonds)
					{
						if (diamond != 0)
						{
							var productDiamond = new ProductDiamond
							{
								IsMain = false,
								DiamondId = diamond,
								ProductId = product.Id,
							};
							await _unitOfWork.ProductDiamondRepository.AddAsync(productDiamond);
							await _unitOfWork.SaveChangeAsync();
						}
					}
				}

				if (!createProduct.ProductImages.IsNullOrEmpty())
				{
					await _imageService.UploadProductImages(createProduct.ProductImages, product.Id);
				}
				if (isSuccess)
				{
					response.Data = createProduct;
					response.Success = true;
					response.Message = "Product created successfully!";
				}
				else
				{
					response.Success = false;
					response.Message = "Error saving the product!";
				}
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
				response.Message = "Error.";
				response.ErrorMessages = new List<string> { ex.Message };
			}
			return response;
		}
		public async Task<ServiceResponse<ProductDTO>> UpdateProductAsync(int id, UpdateProductDTO updatedProduct)
		{
			var response = new ServiceResponse<ProductDTO>();
			var isExisted = await _unitOfWork.ProductRepository.NameIsExisted(updatedProduct.Name);
			if (isExisted)
			{
				response.Success = false;
				response.Message = "Name existed!";
				return response;
			}
			try
			{
				var existProduct = await _unitOfWork.ProductRepository.GetByIdAsync(id);
				if (existProduct == null)
				{
					response.Success = false;
					response.Message = "Product not found.";
					return response;
				}

				if (existProduct.Wage != updatedProduct.Wage)
				{
					existProduct.Price = existProduct.Price - existProduct.Wage + updatedProduct.Wage;
				}

				if (existProduct.ProductTypeId != updatedProduct.ProductTypeId)
				{
					var oldProductType = await _unitOfWork.ProductTypeRepository.GetByIdAsync(existProduct.ProductTypeId);
					var newProductType = await _unitOfWork.ProductTypeRepository.GetByIdAsync(updatedProduct.ProductTypeId);
					if (newProductType != null && oldProductType != null)
					{
						existProduct.Price = (existProduct.Price
						- (oldProductType.Price * existProduct.Weight))
						+ (newProductType.Price * updatedProduct.Weight);
					}
				}

				if (existProduct.Weight != updatedProduct.Weight)
				{
					var productType = await _unitOfWork.ProductTypeRepository.GetByIdAsync(existProduct.ProductTypeId);
					if (productType != null)
					{
						existProduct.Price = existProduct.Price
							- existProduct.Weight * productType.Price
							+ updatedProduct.Weight * productType.Price;
					}
				}

				if (existProduct.Quantity == 0 & updatedProduct.Quantity > 0) existProduct.IsDeleted = false;

				var newProduct = _mapper.Map(updatedProduct, existProduct);
				if (existProduct.Quantity == 0) existProduct.IsDeleted = true;
				_unitOfWork.ProductRepository.Update(existProduct);

				var result = _mapper.Map<ProductDTO>(newProduct);

				var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
				if (isSuccess)
				{
					response.Data = result;
					response.Success = true;
					response.Message = "Product updated successfully.";
				}
				else
				{
					response.Success = false;
					response.Message = "Error updating product.";
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = "Error.";
				response.ErrorMessages = new List<string> { ex.Message };
			}
			return response;
		}
		public async Task<ServiceResponse<bool>> DeleteProductAsync(int id)
		{
			var response = new ServiceResponse<bool>();

			var exist = await _unitOfWork.ProductRepository.GetByIdAsync(id);
			if (exist == null)
			{
				response.Success = false;
				response.Message = "Product is not existed!";
				return response;
			}
			else if (exist.IsDeleted == true)
			{
				response.Success = false;
				response.Message = "Product have been deleted from the system!";
				return response;
			}
			try
			{
				_unitOfWork.ProductRepository.SoftRemove(exist);
				exist.IsDeleted = true;
				var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
				if (isSuccess)
				{
					response.Success = true;
					response.Message = "Product deleted successfully!";
				}
				else
				{
					response.Success = false;
					response.Message = "Error deleting product!";
				}
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