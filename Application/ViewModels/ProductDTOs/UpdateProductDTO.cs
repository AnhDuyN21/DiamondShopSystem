using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ProductDTOs
{
	public class UpdateProductDTO
	{
		public string Name { get; set; }
		public int CategoryId { get; set; }
		public int ProductTypeId { get; set; }
		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Weight must be greater than 0.")]
		public decimal Weight { get; set; }
		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Wage must be greater than 0.")]
		public decimal Wage { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Quantity must be equal or greater than 0.")]
		public int Quantity { get; set; }
	}
}