using BLL.Model.Cart;
using BLL.Model.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Model
{
    public  class CartAndCustomerAndShipper
    {
        public CartDtos cartDtos { get; set; }
        public CustomerDtos customerDtos { get; set; }
        public CustomerDtos shipperDtos { get; set; }
    }
}
