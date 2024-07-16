using Application.Commons;
using Application.Interfaces;
using Application.ViewModels.ProductTypeDTOS;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class ProductTypeService : IProductTypeService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductTypeService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ServiceResponse<IEnumerable<ProductTypeDTO>>> GetProductTypesAsync()
		{
			var response = new ServiceResponse<IEnumerable<ProductTypeDTO>>();
			try
			{
				var types = await _unitOfWork.ProductTypeRepository.GetAllAsync();
				var typeDTOs = new List<ProductTypeDTO>();
				foreach (var type in types)
				{
					typeDTOs.Add(_mapper.Map<ProductTypeDTO>(type));
				}
				if (typeDTOs.Count > 0)
				{
					response.Data = typeDTOs;
					response.Success = true;
					response.Message = "Product types retrieved successfully!";
				}
				else
				{
					response.Success = false;
					response.Message = "No product types were found!";
				}
			}
			catch (DbException ex)
			{
				response.Success = false;
				response.Message = "Database error occurred.";
				response.ErrorMessages = new List<string> { ex.Message };
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = "Error.";
				response.ErrorMessages = new List<string> { ex.Message };
			}
			return response;
		}
		public async Task<ServiceResponse<ProductTypeDTO>> GetProductTypeByIdAsync(int id)
		{
			var response = new ServiceResponse<ProductTypeDTO>();
			var exist = await _unitOfWork.ProductTypeRepository.GetByIdAsync(id);
			var type = _mapper.Map<ProductTypeDTO>(exist);
			if (type == null)
			{
				response.Success = false;
				response.Message = "Cannot found product type.";
			}
			else
			{
				response.Success = true;
				response.Message = "Product type was retrieved successfully!";
				response.Data = type;
			}
			return response;
		}
		public async Task<ServiceResponse<ProductTypeDTO>> CreateProductTypeAsync(CreateProductTypeDTO t)
		{
			var response = new ServiceResponse<ProductTypeDTO>();
			var isExisted = await _unitOfWork.ProductTypeRepository.NameIsExisted(t.Material);
			if (isExisted)
			{
				response.Success = false;
				response.Message = "Material name existed!";
				return response;
			}
			try
			{
				var type = _mapper.Map<ProductType>(t);
				type.IsDeleted = false;
				await _unitOfWork.ProductTypeRepository.AddAsync(type);

				var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
				if (isSuccess)
				{
					var typeDTO = _mapper.Map<ProductTypeDTO>(type);
					response.Data = typeDTO;
					response.Success = true;
					response.Message = "Product type created successfully.";
				}
				else
				{
					response.Success = false;
					response.Message = "Error saving product type.";
				}
			}
			catch (DbException ex)
			{
				response.Success = false;
				response.Message = "Database error occurred.";
				response.ErrorMessages = new List<string> { ex.Message };
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = "Error.";
				response.ErrorMessages = new List<string> { ex.Message };
			}
			return response;
		}
		public async Task<ServiceResponse<ProductTypeDTO>> UpdateProductTypeAsync(int id, CreateProductTypeDTO t)
		{
			var response = new ServiceResponse<ProductTypeDTO>();
			var isExisted = await _unitOfWork.ProductTypeRepository.NameIsExisted(t.Material);
			if (isExisted)
			{
				response.Success = false;
				response.Message = "Material name existed!";
				return response;
			}
			try
			{
				var exist = await _unitOfWork.ProductTypeRepository.GetByIdAsync(id);
				if (exist == null)
				{
					response.Success = false;
					response.Message = "Product type not found.";
					return response;
				}
				if (exist.IsDeleted == true)
				{
					response.Success = false;
					response.Message = "Product type has been deleted from the system.";
					return response;
				}
				var products = await _unitOfWork.ProductRepository.GetAllAsync();
				foreach (var product in products)
				{
					if (product.ProductTypeId == id)
						product.Price = product.Price + product.Weight * t.Price - product.Weight * exist.Price;
				}
				exist = _mapper.Map(t, exist);
				_unitOfWork.ProductTypeRepository.Update(exist);
				var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
				if (isSuccess == true)
				{
					response.Success = true;
					response.Message = "Update successfully.";
					response.Data = _mapper.Map<ProductTypeDTO>(exist);
				}
				else
				{
					response.Success = false;
					response.Message = "Cannot update.";
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = "Error.";
				response.ErrorMessages = new List<string> { ex.Message };
			}
			return response;
		}
		public async Task<ServiceResponse<bool>> DeleteProductTypeAsync(int id)
		{
			var response = new ServiceResponse<bool>();

			var exist = await _unitOfWork.ProductTypeRepository.GetByIdAsync(id);
			if (exist == null)
			{
				response.Success = false;
				response.Message = "Product type is not existed!";
				return response;
			}
			else if (exist.IsDeleted == true)
			{
				response.Success = false;
				response.Message = "Product type have been deleted from the system!";
				return response;
			}

			var isInUsed = await _unitOfWork.ProductRepository.ProductTypeInUsed(id);
			if (isInUsed)
			{
				response.Success = false;
				response.Message = "This ProductType is in used. Delete fail.";
				return response;
			}

			try
			{
				_unitOfWork.ProductTypeRepository.SoftRemove(exist);
				var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
				if (isSuccess)
				{
					response.Success = true;
					response.Message = "Product type deleted successfully!";
				}
				else
				{
					response.Success = false;
					response.Message = "Error deleting product type!";
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = "Error";
				response.ErrorMessages = new List<string> { ex.Message };
			}
			return response;
		}
	}
}