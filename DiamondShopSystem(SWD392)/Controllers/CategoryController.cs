using Application.Interfaces;
using Application.Services;
using Application.ViewModels.CategoryDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShopSystem_SWD392_.Controllers
{
    
    public class CategoryController : BaseController
	{
		private readonly ICategoryService categoryService;
		public CategoryController(ICategoryService categoryService)
		{
			this.categoryService = categoryService;
		}

		[HttpGet]
		public async Task<IActionResult> GetCategories()
		{
			var categories = await categoryService.GetCategoriesAsync();
			return Ok(categories);
		}
		[HttpGet]
		public async Task<IActionResult> GetCategoryById(int id)
		{
			var category = await categoryService.GetCategoryByIdAsync(id);
			return Ok(category);
		}
        [Authorize]
        [HttpPost]
		public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryDTO cat)
		{

			if (ModelState.IsValid)
			{
				var response = await categoryService.CreateCategoryAsync(cat);
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
		public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryDTO cat)
		{
			var result = await categoryService.UpdateCategoryAsync(id, cat);
			if (!result.Success)
			{
				return NotFound(result);
			}
			return Ok(result);
		}
        [Authorize]
        [HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var cat = await categoryService.DeleteCategoryAsync(id);
			if (!cat.Success)
			{
				return NotFound(cat);
			}
			return Ok(cat);
		}
	}
}