﻿using Application.Interfaces;
using Application.Services;
using Application.ViewModels.CategoryDTOs;
using Application.ViewModels.ProductDTOs;
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
	}
}