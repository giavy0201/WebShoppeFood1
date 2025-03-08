using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class AccountStore
    {
        [Key]
        [Column("AccountStoreID")]
        public int Id { get; set; }
        [Required]
        [Column("LoginName")]
        public string LoginName { get; set; }
        [Required]
        [Column("Password")]
        public string Password { get; set; }
        [Required]
        [Column("UserName")]
        public string UserName { get; set; }
        [Column("Role")]
        [MaxLength(50)]
        public string Role { get; set; }  // Admin, Manager, Staff...

        [Range(0, 1)]
        [Column("Status")]
        public int Status { get; set; }  // 0: Inactive, 1: Active

        [Column("IsActive")]
        public bool IsActive { get; set; }  // Indicates if the account is active

        [Column("AccountCreatedAt")]
        public DateTime AccountCreatedAt { get; set; }

        [Column("AccountActivatedAt")]
        public DateTime? AccountActivatedAt { get; set; }

        [Column("Notes")]
        public string? Notes { get; set; }
        public int StoreID { get; set; }
        [ForeignKey("StoreID")]
        public Store Store { get; set; }
        [Column("IsGlobalAdmin")]
        public bool IsGlobalAdmin { get; set; }  // True nếu người dùng có quyền quản lý tất cả cửa hàng
    }
}
