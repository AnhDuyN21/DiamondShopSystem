﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ProductTypeDTOS
{
	public class ProductTypeDTO
	{
		public int Id { get; set; }
		public string Material {  get; set; }
		public decimal Price { get; set; }
		public bool IsDeleted { get; set; }
	}
}
