using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Customer : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        [Range(0, 2)]
        public int? Gender { get; set; }
        public string? Location { get; set; }
        public override string? PhoneNumber {  get; set; }
        public string? Image { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? CodeForgotPassword { get; set; }
        // Thuộc tính Role để phân quyền
        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "User"; // Giá trị mặc định là "User"
        public ICollection<Cart> Carts { get; set; }
    }
}
