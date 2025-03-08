using BLL.Helpers;
using BLL.IService;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Models.Request;
using DAL.Entities;
using DAL.Non_Repository.CartRepo;
using System.Security.Cryptography;
using Azure.Core;
using Remotion.Linq.Clauses;
namespace BLL.Service
   
{
    public class MoMoService : IMoMoService
    {
        private readonly IConfiguration _configuration;
        private readonly ICartRepository _cartService;

        public MoMoService(IConfiguration configuration, ICartRepository cartService)
        {
            _configuration = configuration;
            _cartService = cartService;
        }
        //private readonly string secretKey1 = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
        public async Task<string> CreateMoMoPaymentUrl(double amount, int orderId, string orderInfo)
        {
            var momoConfig = _configuration.GetSection("MoMoPayment");
            string endpoint = momoConfig["Endpoint"];
            string accessKey = momoConfig["AccessKey"];
            string secretKey = momoConfig["SecretKey"];
            string partnerCode = momoConfig["PartnerCode"];
            string redirectUrl = momoConfig["RedirectUrl"];
            string ipnUrl = momoConfig["IpnUrl"];
            string requestId = Guid.NewGuid().ToString();
            var cart = await _cartService.GetCartForPayment(orderId);
            if (cart == null)
            {
                throw new Exception("Cart not found.");
            }
            // Ensure that Amount is formatted correctly as a string with two decimal places
            // string formattedAmount = amount.ToString("0.00");

            // Prepare the request body with the relevant fields
            var requestBody = new MoMoPaymentRequestModel
            {
                PartnerCode = partnerCode,
                AccessKey = accessKey,
                RequestId = requestId,
                Amount = amount.ToString(),
                OrderId = orderId.ToString(),
                OrderInfo = orderInfo,
                RedirectUrl = redirectUrl,
                IpnUrl = ipnUrl, // Ensure the IPN URL is set correctly
                RequestType = "captureWallet",
                ExtraData = "" // Optional extra data, can be left empty
                //PayUrl = ""
                
            };

            // Create the raw signature string to be hashed
            string rawHash = $"accessKey={accessKey}&amount={amount}&extraData={requestBody.ExtraData}&ipnUrl={ipnUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={redirectUrl}&requestId={requestId}&requestType=captureWallet";

            // Generate the signature
            string signature = Utilss.GenerateSignature(rawHash, secretKey);

            // Prepare the request object with the signature
            var requestWithSignature = new
            {
                requestBody.PartnerCode,
                requestBody.AccessKey,
                requestBody.RequestId,
                requestBody.Amount,
                requestBody.OrderId,
                requestBody.OrderInfo,
                requestBody.RedirectUrl,
                requestBody.IpnUrl,
                requestBody.RequestType,
                requestBody.ExtraData,
                signature
                //requestBody.PayUrl
            };

            // Initialize HttpClient and send the POST request
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync(endpoint, requestWithSignature);

            // Check if the response is successful
            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create MoMo payment URL. Response: {errorDetails}");
            }

            // Deserialize the response data
            var responseData = await response.Content.ReadFromJsonAsync<MoMoPaymentResponseModel>();
            if (responseData == null || string.IsNullOrEmpty(responseData.PayUrl))
            {
                throw new Exception("Invalid response or payment URL from MoMo API.");
            }
            if (responseData.ResultCode != 0)  // Assuming 0 is the success code
            {
                throw new Exception($"MoMo payment error: {responseData.Message}");
            }
            // Return the payment URL
            return responseData.PayUrl;
        }
        //public async Task HandleMoMoResponse(MoMoPaymentResponseModel response)
        //{
        //    if (response == null)
        //    {
        //        throw new ArgumentNullException(nameof(response), "Response from MoMo is null.");
        //    }

        //    // Retrieve the cart for the given OrderId
        //    var cart = await _cartService.GetCartForPayment(response.OrderId);
        //    if (cart == null)
        //    {
        //        throw new Exception("Cart not found for the given OrderId.");
        //    }

        //    // Update the cart with payment details
        //    cart.MomoTransactionId = response.TransId; // Store the transaction ID from MoMo
        //                                               //cart.MomoStatus = response.ResultCode == 0 ? "Success" : "Failed"; // Set status based on result code
        //    cart.MomoStatus = response.ResultCode == 0 ? "Thành công" : "Thất bại";
        //    // Update cart status based on payment result
        //    //if (response.ResultCode == 0)
        //    //{
        //    //    cart.Status = 2; // Mark the order as "Completed"
        //    //}
        //    //else
        //    //{
        //    //    cart.Status = 1; // Mark the order as "Pending"
        //    //}

        //    // Log any sub-errors if they exist
        //    if (!string.IsNullOrEmpty(response.Message))
        //    {
        //        Console.WriteLine($"Payment failed: {response.Message}");
        //    }

        //    // Handle the signature validation (optional)
        //    if (!IsValidSignature(response))
        //    {
        //        throw new Exception("Invalid signature from MoMo.");
        //    }

        //    // Save the updated cart data in the database
        //    await _cartService.UpdateMomoStatus(cart.Id, cart.MomoTransactionId, cart.MomoStatus);
        //}
        //private bool IsValidSignature(MoMoPaymentResponseModel response)
        //{
        //    // Implement the signature verification logic here (e.g., using HMAC, a secret key, etc.)
        //    // Example: Compare the response signature with a computed signature from your backend

        //    string computedSignature = ComputeSignature(response);
        //    return computedSignature == response.Signature;
        //}

        //private string ComputeSignature(MoMoPaymentResponseModel response)
        //{
        //    // Implement signature computation logic (usually involves hashing the parameters with your secret key)
        //    //string rawHash = $"accessKey={_configuration["MoMoPayment:AccessKey"]}&amount={response.Amount}&extraData={response.ExtraData}&orderId={response.OrderId}&orderInfo={response.OrderInfo}&partnerCode={_configuration["MoMoPayment:PartnerCode"]}&requestId={response.RequestId}&responseTime={response.ResponseTime}&resultCode={response.ResultCode}&transId={response.TransId}";
        //    //return Utilss.GenerateSignature(rawHash, _configuration["MoMoPayment:SecretKey"]);
        //    //string rawHash = $"accessKey={_configuration["MoMoPayment:AccessKey"]}&amount={response.Amount}&extraData={response.ExtraData}&orderId={response.OrderId}&orderInfo={response.OrderInfo}&partnerCode={_configuration["MoMoPayment:PartnerCode"]}&requestId={response.RequestId}&responseTime={response.ResponseTime}&resultCode={response.ResultCode}&transId={response.TransId}";
        //    //return Utilss.GenerateSignature(rawHash, _configuration["MoMoPayment:SecretKey"]);
        //    string rawHash = $"accessKey={_configuration["MoMoPayment:AccessKey"]}&" +
        //            $"amount={response.Amount}&" +
        //            $"extraData={response.ExtraData}&" +
        //            $"orderId={response.OrderId}&" +
        //            $"orderInfo={response.OrderInfo}&" +
        //            $"partnerCode={_configuration["MoMoPayment:PartnerCode"]}&" +
        //            $"requestId={response.RequestId}&" +
        //            $"responseTime={response.ResponseTime}&" +
        //            $"resultCode={response.ResultCode}&" +
        //            $"transId={response.TransId}";

        //    return Utilss.GenerateSignature(rawHash, _configuration["MoMoPayment:SecretKey"]);
        //}
        public async Task HandleMoMoResponse(MoMoPaymentResponseModel response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response), "Response from MoMo is null.");
            }

            // Log the response received from MoMo
            Console.WriteLine("Received Response from MoMo:");
            Console.WriteLine($"PartnerCode: {response.PartnerCode}, OrderId: {response.OrderId}, Amount: {response.Amount}, TransId: {response.TransId}");

            // Log the received signature for comparison
            Console.WriteLine($"Received Signature: {response.Signature}");

            // Calculate the expected signature using the response details
           // string computedSignature = ComputeSignature(response);
            //Console.WriteLine($"Computed Signature: {computedSignature}");

            // Validate the signature
            //if (!IsValidSignature(response))
            //{
            //    throw new Exception("Invalid signature from MoMo.");
            //}

            // Retrieve the cart for the given OrderId
            var cart = await _cartService.GetCartForPayment(response.OrderId);
            if (cart == null)
            {
                throw new Exception("Cart not found for the given OrderId.");
            }

            // Update the cart with payment details
            cart.MomoTransactionId = response.TransId; // Store the transaction ID from MoMo
            cart.MomoStatus = response.ResultCode == 0 ? "Thành công" : "Thất bại";

            // Log any sub-errors if they exist
            if (!string.IsNullOrEmpty(response.Message))
            {
                Console.WriteLine($"Payment failed: {response.Message}");
            }

            // Save the updated cart data in the database
            await _cartService.UpdateMomoStatus(cart.Id, cart.MomoTransactionId, cart.MomoStatus);
        }

        private bool IsValidSignature(MoMoPaymentResponseModel response)
        {
            string computedSignature = ComputeSignature(response);
            return computedSignature == response.Signature;
        }

        private string ComputeSignature(MoMoPaymentResponseModel response)
        {
            string secretKey = _configuration["MoMoPayment:SecretKey"];

            // Build the raw string used for generating the signature (order must match MoMo's API spec)
            string rawData = $"partnerCode={response.PartnerCode}&" +
                    $"orderId={response.OrderId}&" +
                    $"requestId={response.RequestId}&" +
                    $"amount={response.Amount}&" +
                    $"orderInfo={response.OrderInfo}&" +
                    $"orderType={response.OrderType}&" +
                    $"transId={response.TransId}&" +
                    $"resultCode={response.ResultCode}&" +
                    $"message={response.Message}&" +
                    $"payType={response.PayType}&" +
                    $"responseTime={response.ResponseTime}&" +
                    $"extraData={response.ExtraData}";



            // Append the secret key to the raw data
            rawData += secretKey;

            // Hash the concatenated string using SHA256
            return GenerateSha256Hash(rawData);
        }
        private string GenerateSha256Hash(string rawData)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }


    }
}
