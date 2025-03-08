using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.DTOs.Cart
{
    public class CartPayment
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string CustomerId { get; set; }
        public string Delivery { get; set; }
        public int? PhoneNumber { get; set; }
        public DateTime? TimeOrder { get; set; }
        public string? ShipperId { get; set; }
        //public double? Status { get; set; }
        //public string PaymentMethod { get; set; } // Lưu phương thức thanh toán: "Momo", "VNPay", "PayPal", ...

        ////[Column("PaymentStatus")]
        //public string PaymentStatus { get; set; } // Trạng thái thanh toán: "Pending", "Success", "Failed"

        ////[Column("PaymentTime")]
        //public DateTime? PaymentTime { get; set; } // Thời gian thanh toán
        //public List<Detail> DetailCarts { get; set; }
    }

    //public class Detail
    //{
    //    public int Id { get; set; }
    //    public int FoodId { get; set; }
    //    public int Quantity { get; set; }
    //    public double Price { get; set; }
    //    public int CartID { get; set; }
    //    public string FoodName { get; set; }
    //}
}

