using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Request
{
    public  class MoMoPaymentRequestModel
    {
        public string PartnerCode { get; set; }
        public string AccessKey { get; set; }
        public string RequestId { get; set; }
        public string Amount { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public string RedirectUrl { get; set; }
        public string IpnUrl { get; set; }
        public string RequestType { get; set; }
        public string ExtraData { get; set; }
        //public string PayUrl { get; set; }
    }
}
