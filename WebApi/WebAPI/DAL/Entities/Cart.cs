using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Cart
    {
        [Key]
        [Column("CartID")]
        public int Id { get; set; }
        [Column("StoreID")]
        public int StoreId { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        //[Column("ShipperId")]
        //public string ShipperId { get; set; }  // Thêm thông tin shipper vào đơn hàng
        //[ForeignKey("ShipperId")]
        //public Customer Shipper { get; set; }  // Quan hệ với bảng Customer
        [Column("ShipperId")]
        public string? ShipperId { get; set; } // Lưu thông tin của shipper như một cột
        [Column("DeliveryAddress")]
        public string Delivery { get; set; }
        [Column("DeliveryNoDiacritic")]
        public string? DeliveryNoDiacritic { get; set; }
        [Column("Phone")]
        [Range(0000000000, 9999999999)]
        public int? PhoneNumber { get; set; }
        [Column("TimeOrder")]
        public DateTime? TimeOrder { get; set; }
        [Range(1, 2)]
        public double? Status { get; set; }
        // Thông tin thanh toán
        //[Column("PaymentMethod")]
        //public string PaymentMethod { get; set; } // Lưu phương thức thanh toán: "Momo", "VNPay", "PayPal", ...
        //[Column("PaymentId")]
        //public int? PaymentId { get; set; } // Numeric unique identifier from the payment gateway
        //[Column("PaymentStatus")]
        //public string PaymentStatus { get; set; } // Trạng thái thanh toán: "Pending", "Success", "Failed"

        //[Column("PaymentTime")]
        //public DateTime? PaymentTime { get; set; } // Thời gian thanh toán
        [Column("MomoTransactionId")]
        [MaxLength(100)]
        public string? MomoTransactionId { get; set; } // Mã giao dịch MOMO

        [Column("MomoStatus")]
        [MaxLength(50)]
        public string? MomoStatus { get; set; } // Trạng thái giao dịch: "Pending", "Success", "Failed"
        [Column("MomoResponseTime")]
        //public DateTime? MomoResponseTime { get; set; } // Thời gian phản hồi từ MOMO
        public ICollection<DetailCart> DetailCarts { get; set; }

    }
}
