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
	public class ProductTypeRepository : GenericRepository<ProductType>, IProductTypeRepository
	{
		private readonly AppDbContext _dbContext;
		public ProductTypeRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
		{
			_dbContext = context;
		}
		public Task<bool> NameIsExisted(string name) =>
												 _dbContext.ProductTypes.AnyAsync(x => x.Material == name);
	}
}
