﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ProductDTOs
{
	public class UpdateProductDTO
	{
		public string Name { get; set; }
		public decimal Size { get; set; }
		public decimal Price { get; set; }
		public decimal Wage { get; set; }
		public int ProductTypeId { get; set; }
		public int CategoryId { get; set; }
		public int Quantity { get; set; }
	}
}
