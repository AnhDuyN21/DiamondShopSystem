﻿using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShopSystem_SWD392_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("place-order")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PlaceOrderAsync(string? address, string? phonenumber, int paymentid)
        {
            var result = await _orderService.PlaceOrderAsync(address, phonenumber, paymentid);
            return Ok(result);
        }

        [HttpPost("change-status/{id}")]
        [Authorize(Roles = "SaleStaff,Admin")]
        public async Task<IActionResult> ChangeOrderStatusAsync(int id, string status)
        {
            var result = await _orderService.ChangeOrderStatusAsync(id, status);
            return Ok(result);
        }

        [HttpGet("order/{id}")]
        [Authorize(Roles = "SaleStaff,Admin")]
        public async Task<IActionResult> GetOrderDetailsAsync(int id)
        {
            var result = await _orderService.GetOrderDetailsAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "SaleStaff,Admin")]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var result = await _orderService.GetOrdersAsync();
            return Ok(result);
        }

        [HttpGet("orders/user")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetOrdersForUserAsync()
        {
            var result = await _orderService.GetOrdersForUserAsync();
            return Ok(result);
        }
    }
}
