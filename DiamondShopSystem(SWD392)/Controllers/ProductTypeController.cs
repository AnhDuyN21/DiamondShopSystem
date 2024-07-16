using Application.Interfaces;
using Application.ViewModels.ProductTypeDTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShopSystem_SWD392_.Controllers
{
    
    public class ProductTypeController : BaseController
	{
		private readonly IProductTypeService typeService;
		public ProductTypeController(IProductTypeService typeService)
		{
			this.typeService = typeService;
		}

		[HttpGet]
		public async Task<IActionResult> GetProductTypes()
		{
			var types = await typeService.GetProductTypesAsync();
			return Ok(types);
		}
		[HttpGet]
		public async Task<IActionResult> GetProductTypeById(int id)
		{
			var type = await typeService.GetProductTypeByIdAsync(id);
			return Ok(type);
		}
        [Authorize]
        [HttpPost]
		public async Task<IActionResult> CreateProductType([FromForm] CreateProductTypeDTO type)
		{

			if (ModelState.IsValid)
			{
				var response = await typeService.CreateProductTypeAsync(type);
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
		public async Task<IActionResult> UpdateProductType(int id, [FromBody] CreateProductTypeDTO type)
		{
			var result = await typeService.UpdateProductTypeAsync(id, type);
			if (!result.Success)
			{
				return NotFound(result);
			}
			return Ok(result);
		}
        [Authorize]
        [HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProductType(int id)
		{
			var type = await typeService.DeleteProductTypeAsync(id);
			if (!type.Success)
			{
				return NotFound(type);
			}
			return Ok(type);
		}
	}
}