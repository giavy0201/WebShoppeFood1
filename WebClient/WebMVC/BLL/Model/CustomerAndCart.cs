using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Model.Customer;
using BLL.Model.Cart;
using BLL.Model.ModelStoreDtos;

namespace BLL.Model
{
    public  class CustomerAndCart
    {
        public CustomerDtos1 CustomerDtos1 { get; set; }
        public List<CartDtos> Carts { get; set; }

        public StoreDtos StoreDtos { get; set; }
    }
}
