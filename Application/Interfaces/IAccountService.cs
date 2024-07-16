using Application.Commons;
using Application.ViewModels.AccountDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResponse<IEnumerable<AccountDTO>>> GetUserAsync();
        Task<ServiceResponse<AccountDTO>> GetUserByIdAsync(int id);
        Task<ServiceResponse<AccountDTO>> UpdateUserAsync(int id, UpdateAccountDTO userDTO);
        Task<ServiceResponse<bool>> DeleteUserAsync(int id);
        Task<ServiceResponse<IEnumerable<AccountDTO>>> SearchUserByNameAsync(string name);
        Task<ServiceResponse<AccountDTO>> CreateAccountAsync(CreateAccountDTO createdUserDTO);
        Task<ServiceResponse<bool>> UnDeleteUserAysnc(int id);

    }
}
