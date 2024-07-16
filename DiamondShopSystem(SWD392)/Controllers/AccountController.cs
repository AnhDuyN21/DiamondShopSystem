using Application.Interfaces;
using Application.ViewModels.AccountDTOs;
using Application.ViewModels.DiamondDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShopSystem_SWD392_.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAccountList()
        {
            var User = await _accountService.GetUserAsync();
            return Ok(User);

        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var findaccountUser = await _accountService.GetUserByIdAsync(id);
            if (!findaccountUser.Success)
            {
                return NotFound(findaccountUser);
            }
            return Ok(findaccountUser);
        }
        [Authorize]
        [HttpGet("{name}")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var result = await _accountService.SearchUserByNameAsync(name);

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
        public async Task<IActionResult> CreateUser([FromForm] CreateAccountDTO createdAccountDTO)
        {
          
            // return await _accountService.CreateAccountAsync(createdAccountDTO);
           return Created(nameof(CreateUser), await _accountService.CreateAccountAsync(createdAccountDTO));
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromForm] UpdateAccountDTO accountDTO)
        {
            var updatedUser = await _accountService.UpdateUserAsync(id, accountDTO);
            if (!updatedUser.Success)
            {
                return NotFound(updatedUser);
            }
            return Ok(updatedUser);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UnDeleteAccount(int id)
        {
            var account = await _accountService.UnDeleteUserAysnc(id);
            if (!account.Success)
            {
                return NotFound(account);
            }
            return Ok(account);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var deletedUser = await _accountService.DeleteUserAsync(id);
            if (!deletedUser.Success)
            {
                return NotFound(deletedUser);
            }

            return Ok(deletedUser);
        }

    }
}

