﻿using Application.Commons;
using Application.Interfaces;
using Application.ViewModels.AccountDTOs;
using Application.ViewModels.PromotionDTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace Application.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PromotionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IEnumerable<PromotionDTO>>> GetListPromotionAsync()
        {
            var response = new ServiceResponse<IEnumerable<PromotionDTO>>();
            try
            {
                var list = await _unitOfWork.PromotionRepository.GetAllAsync();

                var promotionList = new List<PromotionDTO>();

                foreach (var promotion in list)
                {

                        promotionList.Add(_mapper.Map<PromotionDTO>(promotion));
                    
                }
                if (promotionList.Count != 0)
                {
                    response.Success = true;
                    response.Message = "Promotion retrieved successfully";
                    response.Data = promotionList;
                }
                else
                {
                    response.Success = true;
                    response.Message = "Not have promotion";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return response;
        }
        public async Task<ServiceResponse<PromotionDTO>> GetPromotionByIdAsync(int id)
        {
            var response = new ServiceResponse<PromotionDTO>();

            var promotion = await _unitOfWork.PromotionRepository.GetByIdAsync(id);

            if (promotion == null )
            {
                response.Success = false;
                response.Message = "Promotion is not existed";
            }
            else
            {
                response.Success = true;
                response.Message = "Promotion found";
                response.Data = _mapper.Map<PromotionDTO>(promotion);
            }

            return response;
        }
        public async Task<ServiceResponse<PromotionDTO>> CreatePromotionAsync(CreatePromotionDTO newPromotion)
        {
            var response = new ServiceResponse<PromotionDTO>();
            var isExisted = await _unitOfWork.PromotionRepository.IsExisted(newPromotion.Point, newPromotion.DiscountPercentage);
            var isValid = await _unitOfWork.PromotionRepository.IsValid(newPromotion.Point, newPromotion.DiscountPercentage);
			if (isExisted)
            {
				response.Success = false;
				response.Message = "Point or DiscountPercentage existed.";
                return response;
			}
            if (!isValid)
            {
				response.Success = false;
				response.Message = "Invalid DiscountPercentage.";
                return response;
			}
            try
            {
                //Mapping
                var promotion = _mapper.Map<Promotion>(newPromotion);

                promotion.IsDeleted = false;

                await _unitOfWork.PromotionRepository.AddAsync(promotion);

                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    var data = _mapper.Map<PromotionDTO>(promotion);
                    response.Data = data;
                    response.Success = true;
                    response.Message = "Promotion created successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error saving the promotion.";
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
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> DeletePromotionAsync(int id)
        {
            var response = new ServiceResponse<bool>();
            var exist = await _unitOfWork.PromotionRepository.GetByIdAsync(id);
            if (exist == null || exist.IsDeleted == true)
            {
                response.Success = false;
                response.Message = "Promotion is not existed";
                return response;
            }
            try
            {
                await _unitOfWork.PromotionRepository.HardDelete(exist.Id);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    response.Success = true;
                    response.Message = "Promotion deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error deleting the promotion.";
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

        public async Task<decimal> DiscountFromPoint(decimal price, int point)
        {
            decimal result = 1;
            decimal discount = 0;

            var getAllPromotionList =  await _unitOfWork.PromotionRepository.GetAllAsync();
            List<Promotion> sortedList = getAllPromotionList.OrderBy(p => p.Point).ToList();

            foreach (var promotion in sortedList)
            {
                if(point >= promotion.Point)
                {
                    discount = promotion.DiscountPercentage;
                }
            }

            result = price - (discount * price);

            return result;
        }

        public async Task<float> GetDiscountPercentageForUser(int userId)
        {
            float discount = 0;

            var user = await _unitOfWork.AccountRepository.GetByIdAsync(userId);
            var getAllPromotionList = await _unitOfWork.PromotionRepository.GetAllAsync();
            List<Promotion> sortedList = getAllPromotionList.OrderBy(p => p.Point).ToList();

            foreach (var promotion in sortedList)
            {
                if (user.Point >= promotion.Point)
                {
                    discount = (float)promotion.DiscountPercentage;
                }
            }

            return discount;
        }
    }
}
