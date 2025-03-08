using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ModelsRequest
{
    public class CreateCustomerReq
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [Range(0, 2, ErrorMessage = "Gender must be 0 (Male), 1 (Female), or 2 (Other).")]
        public int? Gender { get; set; }

        [MaxLength(250)]
        public string? Location { get; set; }

        [DataType(DataType.ImageUrl)]
        public string? Image { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
    }
}
