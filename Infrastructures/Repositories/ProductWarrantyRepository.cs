﻿using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.ProductWarrantyDTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
	public class ProductWarrantyRepository : GenericRepository<ProductWarranty>, IProductWarrantyRepository
	{
		private readonly AppDbContext _dbContext;
		public ProductWarrantyRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
		{
			_dbContext = context;
		}
		public async Task CreateWarrantyByOrderId(int id)
		{
			var order = await _dbContext.Orders
				.Include(o => o.Items)
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			if (order != null)
			{
				foreach (var item in order.Items)
				{
					var warranty = new ProductWarranty
					{
						OrderId = order.Id,
						ProductId = item.ProductId,
						StartDate = DateTime.Now,
						EndDate = DateTime.Now.AddYears(1),
						Description = "None"
					};
					await AddAsync(warranty);
				}
			}
			await _dbContext.SaveChangesAsync();
		}
		public async Task<ProductWarrantyDTO> GetWarrantyByItem(int orderId, int productId)
		{
			var warranty = await _dbContext.ProductWarranties.Where(x => x.OrderId == orderId && x.ProductId == productId).FirstOrDefaultAsync();
			if (warranty != null)
			{
				return new ProductWarrantyDTO
				{
					OrderId = orderId,
					ProductId = productId,
					Description = warranty.Description,
					StartDate = warranty.StartDate,
					EndDate = warranty.EndDate,
				};
			}
			return new ProductWarrantyDTO();
		}
	}
}