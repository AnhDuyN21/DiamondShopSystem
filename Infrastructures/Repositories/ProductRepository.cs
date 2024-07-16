using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
	public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		private readonly AppDbContext _dbContext;
		public ProductRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
		{
			_dbContext = context;
		}

		public async Task<IEnumerable<Product>> SearchProduct(string search)
		{
			return await _dbContext.Products.Where(u => u.Name.Contains(search)).ToListAsync();
		}
		public async Task<bool> ProductTypeInUsed(int id)
		{
			var product = await _dbContext.Products.Where(x => x.ProductTypeId == id).FirstOrDefaultAsync();
			return product != null;
		}
		public async Task<bool> CategoryInUsed(int id)
		{
			var product = await _dbContext.Products.Where(x => x.CategoryId == id).FirstOrDefaultAsync();
			return product != null;
		}
		public Task<bool> NameIsExisted(string name) =>
												 _dbContext.Products.AnyAsync(x => x.Name == name);
	}
}