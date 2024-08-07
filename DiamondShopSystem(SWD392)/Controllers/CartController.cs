﻿using Application.Interfaces;
using Application.RequestModel.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShopSystem_SWD392_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("view-cart")]
        [Authorize]
        public async Task<IActionResult> GetCartItemsForUser()
        {
            var result = await _cartService.GetCartForUserAsync();
            return Ok(result);
        }

        [HttpPatch("add-to-cart")]
        [Authorize]
        public async Task<IActionResult> AddToCartAsync(CartRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _cartService.AddToCartAsync(request);
            return Ok(result);
        }

        [HttpPatch("remove-from-cart")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCartAsync(CartRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _cartService.RemoveFromCartAsync(request);
            return Ok(result);
        }

        [HttpDelete("delete-cart")]
        [Authorize]
        public async Task<IActionResult> DeleteCartAsync()
        {
            var result = await _cartService.DeleteCartAsync();
            return Ok(result);
        }
    }
}
