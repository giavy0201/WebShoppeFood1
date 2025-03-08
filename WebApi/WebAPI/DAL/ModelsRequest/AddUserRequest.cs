using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ModelsRequest
{
    public class AddUserRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public DateTime? Birthday { get; set; }

        [Range(0, 2)]
        public int? Gender { get; set; }

        public string? Location { get; set; }

        public string? Image { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
