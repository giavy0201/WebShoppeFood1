using BLL.IService;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Models.DTOs.Cart;
namespace BLL.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ICartService _cartService;
        public PaymentService(IConfiguration configuration, HttpClient httpClient, ICartService cartService)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _cartService = cartService;
        }

        //public string CreatePaymentUrl(Task<CartPayment> cart)
        //{
        //    var vnpay = new VnPayLibrary();

        //    // Get configuration values
        //    string vnpVersion = _configuration["VNPay:Version"];
        //    string vnpCommand = _configuration["VNPay:Command"];
        //    string vnpTmnCode = _configuration["VNPay:TmnCode"];
        //    string vnpHashSecret = _configuration["VNPay:HashSecret"];
        //    string vnpReturnUrl = _configuration["VNPay:ReturnUrl"];

        //    // Generate unique order ID
        //    string orderCode = DateTime.Now.Ticks.ToString();

        //    // Prepare payment parameters
        //    vnpay.AddRequestData("vnp_Version", vnpVersion);
        //    vnpay.AddRequestData("vnp_Command", vnpCommand);
        //    vnpay.AddRequestData("vnp_TmnCode", vnpTmnCode);
        //    vnpay.AddRequestData("vnp_Amount", (cart.detai.Sum(x => x.Price * x.Quantity) * 100).ToString()); // Amount in VND
        //    vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
        //    vnpay.AddRequestData("vnp_CurrCode", "VND");
        //    vnpay.AddRequestData("vnp_IpAddr", "127.0.0.1"); // Customer IP
        //    vnpay.AddRequestData("vnp_Locale", "vn");
        //    vnpay.AddRequestData("vnp_OrderInfo", $"Payment for order {cart.Id}");
        //    vnpay.AddRequestData("vnp_OrderType", "other");
        //    vnpay.AddRequestData("vnp_ReturnUrl", vnpReturnUrl);
        //    vnpay.AddRequestData("vnp_TxnRef", orderCode);

        //    // Generate payment URL
        //    return vnpay.CreateRequestUrl(vnpTmnCode, vnpHashSecret);
        //}

        //public async Task<PaymentResponse> ProcessPaymentResponse(IQueryCollection queryCollection)
        //{
        //    var vnpay = new VnPayLibrary();
        //    var vnpHashSecret = _configuration["VNPay:HashSecret"];

        //    // Verify payment response
        //    var isValidSignature = vnpay.ValidateSignature(queryCollection, vnpHashSecret);

        //    if (!isValidSignature)
        //    {
        //        return new PaymentResponse
        //        {
        //            Success = false,
        //            Message = "Invalid payment signature"
        //        };
        //    }

        //    string vnpResponseCode = queryCollection["vnp_ResponseCode"];
        //    string vnpTransactionStatus = queryCollection["vnp_TransactionStatus"];

        //    // Determine payment status
        //    bool paymentSuccessful = vnpResponseCode == "00" && vnpTransactionStatus == "00";

        //    return new PaymentResponse
        //    {
        //        Success = paymentSuccessful,
        //        TransactionId = queryCollection["vnp_TransactionNo"],
        //        OrderId = queryCollection["vnp_TxnRef"],
        //        Amount = long.Parse(queryCollection["vnp_Amount"]) / 100,
        //        Message = paymentSuccessful ? "Payment successful" : "Payment failed"
        //    };
        //}
        //        public async Task<string> CreatePaymentUrl(int cartId)
        //        {
        //            // Retrieve cart details
        //            var cart = await _cartService.GetCartForPayment(cartId);
        //            if (cart == null)
        //            {
        //                throw new Exception("Cart not found");
        //            }

        //            // Calculate total amount
        //            var totalAmount = cart.DetailCarts.Sum(x => x.Price * x.Quantity);

        //            var vnpay = new VnPayLibrary();

        //            // Get configuration values
        //            string vnpVersion = _configuration["VNPay:Version"];
        //            string vnpCommand = _configuration["VNPay:Command"];
        //            string vnpTmnCode = _configuration["VNPay:TmnCode"];
        //            string vnpHashSecret = _configuration["VNPay:HashSecret"];
        //            string vnpReturnUrl = _configuration["VNPay:ReturnUrl"];

        //            // Generate unique order code
        //            string orderCode = cart.Id.ToString(); // Using cart ID as order reference

        //            // Prepare payment parameters
        //            vnpay.AddRequestData("vnp_Version", vnpVersion);
        //            vnpay.AddRequestData("vnp_Command", vnpCommand);
        //            vnpay.AddRequestData("vnp_TmnCode", vnpTmnCode);
        //            vnpay.AddRequestData("vnp_Amount", ((long)(totalAmount * 100)).ToString()); // Amount in VND (multiplied by 100)
        //            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
        //            vnpay.AddRequestData("vnp_CurrCode", "VND");
        //            vnpay.AddRequestData("vnp_IpAddr", "127.0.0.1"); // Customer IP
        //            vnpay.AddRequestData("vnp_Locale", "vn");
        //            vnpay.AddRequestData("vnp_OrderInfo", $"Payment for cart {cart.Id}");
        //            vnpay.AddRequestData("vnp_OrderType", "other");
        //            vnpay.AddRequestData("vnp_ReturnUrl", vnpReturnUrl);
        //            vnpay.AddRequestData("vnp_TxnRef", orderCode);

        //            // Update cart payment status to pending
        //            await _cartService.UpdatePaymentStatus(
        //                cartId,
        //                "Pending",
        //                "VNPay",
        //                null
        //            );

        //            // Generate payment URL
        //            return vnpay.CreateRequestUrl(vnpTmnCode, vnpHashSecret);
        //        }

        //        public async Task<PaymentResult> ProcessPaymentResponse(IQueryCollection queryCollection)
        //        {
        //            var vnpay = new VnPayLibrary();
        //            var vnpHashSecret = _configuration["VNPay:HashSecret"];

        //            // Verify payment response
        //            var isValidSignature = vnpay.ValidateSignature(queryCollection, vnpHashSecret);

        //            if (!isValidSignature)
        //            {
        //                return new PaymentResult
        //                {
        //                    Success = false,
        //                    Message = "Invalid payment signature"
        //                };
        //            }

        //            // Extract payment details
        //            string vnpResponseCode = queryCollection["vnp_ResponseCode"];
        //            string transactionNo = queryCollection["vnp_TransactionNo"];
        //            string orderInfo = queryCollection["vnp_TxnRef"];

        //            // Parse cart ID from order reference
        //            if (!int.TryParse(orderInfo, out int cartId))
        //            {
        //                return new PaymentResult
        //                {
        //                    Success = false,
        //                    Message = "Invalid cart ID"
        //                };
        //            }

        //            // Determine payment status
        //            bool paymentSuccessful = vnpResponseCode == "00";

        //            // Update payment status in database
        //            await _cartService.UpdatePaymentStatus(
        //                cartId,
        //                paymentSuccessful ? "Success" : "Failed",
        //                "VNPay",
        //                DateTime.Now
        //            );

        //            return new PaymentResult
        //            {
        //                Success = paymentSuccessful,
        //                CartId = cartId,
        //                TransactionId = transactionNo,
        //                Message = paymentSuccessful ? "Payment successful" : "Payment failed"
        //            };
        //        }
        //    }

        //    // Payment result class
        //    public class PaymentResult
        //    {
        //        public bool Success { get; set; }
        //        public int CartId { get; set; }
        //        public string TransactionId { get; set; }
        //        public string Message { get; set; }
        //    }

        //    // Existing VnPayLibrary and VnPayCompare classes remain the same as in the previous implementation
        //}
    }
}

