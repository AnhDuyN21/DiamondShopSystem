using Application.Interfaces;
using Application.Services;
using Application.ViewModels.AccountDTOs;
using Application.ViewModels.DiamondDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShopSystem_SWD392_.Controllers
{
    [Authorize]
    public class DiamondController : BaseController
    {
        private readonly IDiamondService _diamondService;
        public DiamondController(IDiamondService diamondService)
        {
            _diamondService = diamondService;
        }
        [HttpGet]
        public async Task<IActionResult> GetDiamondList()
        {
            var Diamond = await _diamondService.GetDiamondAsync();
            return Ok(Diamond);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetDiamondById(int id)
        {
            var diamond = await _diamondService.GetDiamondByIdAsync(id);
            if (!diamond.Success)
            {
                return NotFound(diamond);
            }
            return Ok(diamond);
        }
        [HttpGet]
        [Route("{origin}")]
        public async Task<IActionResult> SearchDiamondByOrigin(string origin)
        {
            var result = await _diamondService.SearchDiamondByOriginAsync(origin);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateDiamond([FromForm] CreateDiamondDTO createdDiamondDTO)
        {

            if (ModelState.IsValid)
            {
                var response = await _diamondService.CreateDiamondAsync(createdDiamondDTO);
                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            else
            {
                return BadRequest("Invalid request data.");
            }
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateDiamond(int id, [FromForm] UpdateDiamondDTO diamondDTO)
        {
            var updatedDiamond = await _diamondService.UpdateDiamondAsync(id, diamondDTO);
            if (!updatedDiamond.Success)
            {
                return NotFound(updatedDiamond);
            }
            return Ok(updatedDiamond);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteDiamond(int id)
        {
            var deletedDiamond = await _diamondService.DeleteDiamondAsync(id);
            if (!deletedDiamond.Success)
            {
                return NotFound(deletedDiamond);
            }

            return Ok(deletedDiamond);
        }
    }
}
