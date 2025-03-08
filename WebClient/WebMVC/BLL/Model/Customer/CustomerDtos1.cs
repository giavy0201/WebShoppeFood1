using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Model.Customer
{
    public class CustomerDtos1
    {
        public string CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public int? Gender { get; set; }
        public string? Location { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Image { get; set; }
    }
}
