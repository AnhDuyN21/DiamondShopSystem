using Application.Interfaces;
using Application.Services;
using Application.ViewModels.AccountDTOs;
using Application.ViewModels.PromotionDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShopSystem_SWD392_.Controllers
{
    public class PromotionController : BaseController
    {
        private readonly IPromotionService _promotionService;
        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }
        [HttpGet]
        
        public async Task<IActionResult>GetPromotionList()
        {
            var list = await _promotionService.GetListPromotionAsync();
            return Ok(list);
        }
        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetPromotionById(int id)
        {
            var promotion = await _promotionService.GetPromotionByIdAsync(id);
            if (!promotion.Success)
            {
                return NotFound(promotion);
            }
            return Ok(promotion);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePromotionAsync([FromForm] CreatePromotionDTO newPromotion)
        {
            var result = await _promotionService.CreatePromotionAsync(newPromotion);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            var deletePromotion = await _promotionService.DeletePromotionAsync(id);
            if (!deletePromotion.Success)
            {
                return NotFound(deletePromotion);
            }

            return Ok(deletePromotion);
        }

    }
}
