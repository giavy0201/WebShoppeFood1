﻿namespace BLL.Models.DTOs.Cart
{
    public class CartSystem
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string CustomerId { get; set; }
        public string Delivery { get; set; }
        public int? PhoneNumber { get; set; }
        public DateTime? TimeOrder { get; set; }
        public string? ShipperId { get; set; }
        public string? MomoStatus { get; set; }
        public double? Status { get; set; }
        public List<SelectCart> DetailCarts { get; set; }
    }

    public class SelectCart
    {
        public int Id { get; set; }
        public int FoodId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int CartID { get; set; }
        public string FoodName { get; set; }
    }
}
