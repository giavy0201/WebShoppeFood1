using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Model.Cart;
using BLL.Model.ModelStoreDtos;
namespace BLL.Model
{
    public  class DetailCartAndStore
    {
        public StoreDtos Store { get; set; }
        public CartDtos Cart { get; set; }
    }
}
