using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.DTOs.AccountStore
{
    public  class AccountDtos
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; } // Admin, Manager, Staff...
        public int Status { get; set; } // 0: Inactive, 1: Active
        public bool IsActive { get; set; } // Indicates if the account is active
        public DateTime AccountCreatedAt { get; set; }
        public DateTime? AccountActivatedAt { get; set; }
        public string? Notes { get; set; }
        public int StoreID { get; set; }
        public string StoreName { get; set; } // Optionally include the store's name
    }
}
