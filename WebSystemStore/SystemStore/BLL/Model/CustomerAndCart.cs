using BLL.Model.Cart;
using BLL.Model.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Model
{
    public  class CustomerAndCart
    {
        public CustomerDtos CustomerDtos { get; set; }
        public List<CartDtos> CartDtos { get; set; }
    }
}
