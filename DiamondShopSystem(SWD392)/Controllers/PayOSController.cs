﻿using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;

namespace DiamondShopSystem_SWD392_.Controllers
{
    public class PayOSController : BaseController
    {
        private readonly PayOS _payOS;
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        public PayOSController(PayOS payOS, IOrderService orderService, IProductService productService, ICartService cartService)
        {
            _payOS = payOS;
            _orderService = orderService;
            _productService = productService;
            _cartService = cartService;
        }
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            try
            {
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                List<ItemData> items = [];
               var cartUser = await _cartService.GetCartForUserAsync();

                var productList = cartUser.Data.Items;

                foreach(var o in productList)
                {
                     //Ép kiểu int? thành int
                    int productId = o.ProductId;

                    //Lấy product với productID trong OrderDetail
                    var product = await _productService.GetProductByIdAsync(productId);

                    //Gán Name product có được
                    string itemName = o.ProductName;

                    //Gán Quantity từ OrderDetail
                    int quantity = o.Quantity;

                    //Gán Price từ OrderDetail
                    int price = (int)o.Price;

                    // Khởi tạo đối tượng ItemData với các giá trị đã lấy được
                    ItemData item = new(itemName, quantity, price);
                    items.Add(item);
                }

                var successUrl = "https://icpih.com/media-intestinal-health-ihsig/PAYMENT-SUCCESS.png";
                var cancelUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTf6Z7CgLJ3JfLy4IREsARVyxcBnQQnHN40jw&s";

                // Tạo đối tượng PaymentData để gửi yêu cầu thanh toán
                PaymentData paymentData = new PaymentData(orderCode, (int)cartUser.Data.TotalPrice, "Thanh toán đơn hàng", items, cancelUrl, successUrl);
                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                return Ok(new
                {
                    message = "redirect",
                    url = createPayment.checkoutUrl
                });
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Redirect("https://localhost:5001/swagger/index.html");
            }
        }
        //[HttpGet]
        //public async Task<IActionResult> Success( [FromQuery] int orderId, [FromQuery] string status)
        //{
        //    if (status == "Cancel")
        //    {
        //        var cancel = await _orderService.ChangeOrderStatusAsync(orderId,"Cancel");
        //        return Redirect("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTf6Z7CgLJ3JfLy4IREsARVyxcBnQQnHN40jw&s");
        //    }
        //      await _orderService.ChangeOrderStatusAsync(orderId, "Paid");
        //    return Redirect("https://icpih.com/media-intestinal-health-ihsig/PAYMENT-SUCCESS.png");
        //}
    }
}

