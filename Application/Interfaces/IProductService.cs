using Application.Commons;
using Application.ViewModels.ProductDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface IProductService
	{
		Task<ServiceResponse<IEnumerable<ProductDTO>>> GetProductsAsync(int? categoryId = null);
		Task<ServiceResponse<ProductDTO>> GetProductByIdAsync(int id);
		Task<ServiceResponse<ProductDTO>> UpdateProductAsync(int id, UpdateProductDTO pro);
		Task<ServiceResponse<bool>> DeleteProductAsync(int id);
		Task<ServiceResponse<IEnumerable<ProductDTO>>> SearchProductByNameAsync(string search);
		Task<ServiceResponse<CreateProductDTO>> CreateProductAsync(CreateProductDTO pro);
	}
}
