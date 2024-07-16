using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountDTOs
{
    public class AccountDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Address {  get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public int? RoleId { get; set; }
        public decimal Point {  get; set; }
        public bool IsDeleted {  get; set; }

    }
}
