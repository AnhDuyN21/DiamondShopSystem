using Application.Interfaces;
using Application.Services;
using Application.ViewModels.ProductDTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShopSystem_SWD392_.Controllers
{
    
    public class ProductController : BaseController
	{
		private readonly IProductService productService;
		public ProductController(IProductService productService)
		{
			this.productService = productService;
		}

		[HttpGet]
		public async Task<IActionResult> GetProducts(int? categoryId)
		{
			var products = await productService.GetProductsAsync(categoryId);
			return Ok(products);
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> GetProductById(int id)
		{
			var product = await productService.GetProductByIdAsync(id);
			if (!product.Success)
			{
				return NotFound(product);
			}
			return Ok(product);
		}

		[HttpGet]
		[Route("{name}")]
		public async Task<IActionResult> SearchProductByName(string name)
		{
			var result = await productService.SearchProductByNameAsync(name);
			if (result.Success)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}
        [Authorize]
        [HttpPost]
		public async Task<IActionResult> CreateProduct([FromForm] CreateProductDTO createProduct)
		{

			if (ModelState.IsValid)
			{
				var response = await productService.CreateProductAsync(createProduct);
				if (response.Success)
				{
					return Ok(response);
				}
				else
				{
					return BadRequest(response);
				}
			}
			else
			{
				return BadRequest("Invalid request data.");
			}
		}
        [Authorize]
        [HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDTO)
		{
			var result = await productService.UpdateProductAsync(id, productDTO);
			if (!result.Success)
			{
				return NotFound(result);
			}
			return Ok(result);
		}
        [Authorize]
        [HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var product = await productService.DeleteProductAsync(id);
			if (!product.Success)
			{
				return NotFound(product);
			}
			return Ok(product);
		}
	}
}