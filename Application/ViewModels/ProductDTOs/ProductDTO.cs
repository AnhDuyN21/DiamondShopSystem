﻿using Application.ViewModels.CategoryDTOs;
using Application.ViewModels.DiamondDTOs;
using Application.ViewModels.ProductTypeDTOS;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ProductDTOs
{
	public class ProductDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CategoryId { get; set; }
		public CategoryDTO Category { get; set; }
		public int ProductTypeId { get; set; }
		public ProductTypeDTO ProductType { get; set; }
		public decimal Weight { get; set; }
		public decimal Wage { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public bool IsDeleted { get; set; }
		public List<DiamondDTO> PrimaryDiamonds { get; set; }
		public List<DiamondDTO> SubDiamonds { get; set; }
		public List<string> Images { get; set; }
	}
}