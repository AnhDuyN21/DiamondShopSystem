using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
	public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
	{
		private readonly AppDbContext _dbContext;
		public CategoryRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
		{
			_dbContext = context;
		}
		public async Task<bool> NameIsExisted(string name, float size, float length)
		{
			//var isName = await _dbContext.Categories.AnyAsync(x => x.Name == name);
			//if (isName) return true;
			if (size == 0)
			{
				var categories = await _dbContext.Categories.Where(x => x.Name == name).ToListAsync();
				foreach (var category in categories)
				{
					if (category.Length == length) return true;
				}
			}
			if (length == 0)
			{
				var categories = await _dbContext.Categories.Where(x => x.Name == name).ToListAsync();
				foreach (var category in categories)
				{
					if (category.Size == size) return true;
				}
			}
			return false;
		}
	}
}