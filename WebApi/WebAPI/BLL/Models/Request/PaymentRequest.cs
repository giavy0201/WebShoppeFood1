using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Request
{
    public  class PaymentRequest
    {
        public double Amount { get; set; }
        public int OrderId { get; set; }
        public string OrderInfo { get; set; }
      
    }
}
