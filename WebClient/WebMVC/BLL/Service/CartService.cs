using BLL.IService;
using BLL.Model;
using BLL.Model.Cart;
using BLL.Model.ModelRequest;
using BLL.Model.Customer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BLL.Model.ModelStoreDtos;
using System.Text.Json;


namespace BLL.Service
{
    public class CartService : ICartService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
       



        public CartService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;

            
        }

        public async Task<List<int>> GetOrCreateCartIdByEmail(string email)
        {
            var url = _configuration["https:localAPI"] + "Cart/GetOrCreateCartId/" + email;
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not retrieve or create cart IDs");
            }

            var content = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(content);

            if (responseObject.isSuccess == false)
            {
                throw new Exception("Failed to get cart IDs from the response: " + responseObject.message);
            }

            var data = responseObject.data;
            var cartIds = new List<int>();

            foreach (var id in data)
            {
                cartIds.Add((int)id);
            }

            return cartIds;
        }
        public async Task<List<CartDtos>> GetCartOrderStatus(int statusId)
        {
            //var url = $"{_configuration["https:localAPI"]}Cart/Status?status={statusId}";

            //try
            //{
            //    // Gửi yêu cầu GET đến API
            //    var response = await _httpClient.GetAsync(url);

            //    // Kiểm tra phản hồi từ API
            //    if (!response.IsSuccessStatusCode)
            //    {
            //        // Nếu không thành công, trả về một thông báo chi tiết về lỗi
            //        var errorMessage = $"API trả về mã lỗi: {response.StatusCode} - {response.ReasonPhrase}";
            //        throw new Exception($"Không thể truy xuất danh sách đơn hàng từ API. {errorMessage}");
            //    }

            //    // Đọc nội dung phản hồi
            //    var content = await response.Content.ReadAsStringAsync();

            //    // Giải mã JSON từ phản hồi
            //    var responseObject = JsonConvert.DeserializeObject<dynamic>(content);

            //    // Kiểm tra trạng thái thành công của API
            //    if (responseObject.isSuccess == false)
            //    {
            //        throw new Exception("API trả về lỗi: " + responseObject.message);
            //    }

            //    // Lấy dữ liệu danh sách đơn hàng (nếu data bằng null thì trả về thông báo không có đơn hàng)
            //    var data = responseObject.data;

            //    if (data == null)
            //    {
            //        // Trả về thông báo không có đơn hàng
            //        throw new Exception("Không có đơn hàng nào với trạng thái này.");
            //    }

            //    // Chuyển đổi danh sách đơn hàng thành danh sách CartDtos
            //    var carts = JsonConvert.DeserializeObject<List<CartDtos>>(JsonConvert.SerializeObject(data));

            //    return carts;
            //}
            //catch (HttpRequestException httpEx)
            //{
            //    // Xử lý lỗi kết nối API (ví dụ: DNS, mạng, hoặc server không phản hồi)
            //    throw new Exception($"Lỗi kết nối đến API: {httpEx.Message}");
            //}
            //catch (Exception ex)
            //{
            //    // Xử lý các lỗi khác
            //    throw new Exception($"Đã xảy ra lỗi: {ex.Message}");
            //}
            var url = $"{_configuration["https:localAPI"]}Cart/Status?status={statusId}";
           
                var response = await _httpClient.GetAsync(url);
                //if (!response.IsSuccessStatusCode)
                //{
                //    var errorMessage = $"API returned error: {response.StatusCode} - {response.ReasonPhrase}";
                //    throw new Exception($"Unable to retrieve order list from API. {errorMessage}");
                //}

                var content = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(content);

                //if (responseObject.isSuccess == false)
                //{
                //    throw new Exception($"API error: {responseObject.message}");
                //}

                var data = responseObject.data;
                //if (data == null)
                //{
                //    // Handle case where no orders are found
                //    throw new Exception("No orders found with this status.");
                //}

                var carts = JsonConvert.DeserializeObject<List<CartDtos>>(JsonConvert.SerializeObject(data));
                return carts;
            
           
            
        }
        public async Task<List<CartDtos>> GetCartOrderStatusDone(int statusId,string shipper)
        {
            //var url = $"{_configuration["https:localAPI"]}Cart/Status?status={statusId}";

            //try
            //{
            //    // Gửi yêu cầu GET đến API
            //    var response = await _httpClient.GetAsync(url);

            //    // Kiểm tra phản hồi từ API
            //    if (!response.IsSuccessStatusCode)
            //    {
            //        // Nếu không thành công, trả về một thông báo chi tiết về lỗi
            //        var errorMessage = $"API trả về mã lỗi: {response.StatusCode} - {response.ReasonPhrase}";
            //        throw new Exception($"Không thể truy xuất danh sách đơn hàng từ API. {errorMessage}");
            //    }

            //    // Đọc nội dung phản hồi
            //    var content = await response.Content.ReadAsStringAsync();

            //    // Giải mã JSON từ phản hồi
            //    var responseObject = JsonConvert.DeserializeObject<dynamic>(content);

            //    // Kiểm tra trạng thái thành công của API
            //    if (responseObject.isSuccess == false)
            //    {
            //        throw new Exception("API trả về lỗi: " + responseObject.message);
            //    }

            //    // Lấy dữ liệu danh sách đơn hàng (nếu data bằng null thì trả về thông báo không có đơn hàng)
            //    var data = responseObject.data;

            //    if (data == null)
            //    {
            //        // Trả về thông báo không có đơn hàng
            //        throw new Exception("Không có đơn hàng nào với trạng thái này.");
            //    }

            //    // Chuyển đổi danh sách đơn hàng thành danh sách CartDtos
            //    var carts = JsonConvert.DeserializeObject<List<CartDtos>>(JsonConvert.SerializeObject(data));

            //    return carts;
            //}
            //catch (HttpRequestException httpEx)
            //{
            //    // Xử lý lỗi kết nối API (ví dụ: DNS, mạng, hoặc server không phản hồi)
            //    throw new Exception($"Lỗi kết nối đến API: {httpEx.Message}");
            //}
            //catch (Exception ex)
            //{
            //    // Xử lý các lỗi khác
            //    throw new Exception($"Đã xảy ra lỗi: {ex.Message}");
            //}
            var url = $"{_configuration["https:localAPI"]}Cart/StatusDone?status={statusId}&shipper={shipper}";

            var response = await _httpClient.GetAsync(url);
            //if (!response.IsSuccessStatusCode)
            //{
            //    var errorMessage = $"API returned error: {response.StatusCode} - {response.ReasonPhrase}";
            //    throw new Exception($"Unable to retrieve order list from API. {errorMessage}");
            //}

            var content = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(content);

            //if (responseObject.isSuccess == false)
            //{
            //    throw new Exception($"API error: {responseObject.message}");
            //}

            var data = responseObject.data;
            //if (data == null)
            //{
            //    // Handle case where no orders are found
            //    throw new Exception("No orders found with this status.");
            //}

            var carts = JsonConvert.DeserializeObject<List<CartDtos>>(JsonConvert.SerializeObject(data));
            return carts;



        }
        public async Task<ApiRequest> UpdateStatusOrder(SetStatusRequest setReq)
        {
            var req = new ApiRequest();
            var url = _configuration["https:localAPI"] + "Cart/Status";
            string data = JsonConvert.SerializeObject(setReq);
            var jsondata = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, jsondata);
            if (!response.IsSuccessStatusCode)
            {
                return req;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var request = JsonConvert.DeserializeObject<ApiResponse<ApiRequest>>(content);
                return request.Data;
            }
        }
        public async Task<CartDtos> SelectCartByID(int CartID)
        {
            var url = _configuration["https:localAPI"] + "Cart/" + CartID;
            var data = await _httpClient.GetAsync(url);
            if (!data.IsSuccessStatusCode)
            {
                return null;
            }
            else
            {
                var content = await data.Content.ReadAsStringAsync();
                var detailcart = JsonConvert.DeserializeObject<ApiResponse<CartDtos>>(content);
                return detailcart.Data;
            }
        }
        public async Task<CartDtos> GetOrderHistory(int CartId)
        {
            var url = _configuration["https:localAPI"] + "Cart/" + CartId;
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                // Xử lý trường hợp không thành công
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject<ApiResponse<CartDtos>>(content);
            return order.Data;
        }
        public async Task<List<CartDtos>> GetOrderHistoryByEmail(string email)
        {
            try
            {
                var cartIds = await GetOrCreateCartIdByEmail(email);

                var orders = new List<CartDtos>();
                foreach (var cartId in cartIds)
                {
                    var order = await GetOrderHistory(cartId);
                    if (order != null)
                    {
                        orders.Add(order);
                    }
                }
                return orders;
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                // Optionally, handle the error gracefully
                return new List<CartDtos>(); // Return an empty list or handle as needed
            }

        }

        public async Task<List<CartDtos>> GetCartsByShipperConfirmedStatus(string shipper)
        {
            try
            {
                // URL for retrieving carts with shipper confirmed status
                //var url = _configuration["https:localAPI"]} Cart / GetCartsWithShipperInfo ? shipper ={ shipper}''; // Pass status as a query parameter
            var url = $"{_configuration["https:localAPI"]}Cart/GetCartsWithShipperInfo?shipper={shipper}";
            var response = await _httpClient.GetAsync(url);

                //if (!response.IsSuccessStatusCode)
                //{
                //    // If the API call was not successful, return an empty list
                //    return new List<CartDtos>();
                //}

                // Deserialize the response content into the list of CartDtos
                var content = await response.Content.ReadAsStringAsync();
                var carts = JsonConvert.DeserializeObject<ApiResponse<List<CartDtos>>>(content);
                return carts.Data;
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, error handling, etc.)
                return new List<CartDtos>(); // Return an empty list in case of error
            }
        }

        public async Task<CustomerDtos> GetShipperInfoByCartId(int cartId)
        {
            try
            {
                // URL for retrieving shipper info based on cartId
                var url = _configuration["https:localAPI"] + "Cart/GetShipperInfoByCartId/" + cartId;
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle the case where the API call was not successful
                    return null;
                }

                // Deserialize the response content into the CustomerDtos object
                var content = await response.Content.ReadAsStringAsync();
                var shipperInfo = JsonConvert.DeserializeObject<ApiResponse<CustomerDtos>>(content);
                return shipperInfo.Data;
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, error handling, etc.)
                return null; // Return null in case of error
            }
        }

        public async Task<bool> AcceptOrder(int cartId, string userId)
        {
            try
            {
                // URL for accepting the order
                var url = _configuration["https:localAPI"] + "Cart/AcceptOrder/" + cartId + "/" + userId;
                var response = await _httpClient.PostAsync(url, null); // Assuming no request body for this API call

                if (!response.IsSuccessStatusCode)
                {
                    // If the request was not successful, return false
                    return false;
                }

                // If the request was successful, return true
                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, error handling, etc.)
                return false; // Return false in case of error
            }
        }

        public async Task<bool> CompleteOrder(int cartId, string userId)
        {
            try
            {
                // URL for accepting the order
                var url = _configuration["https:localAPI"] + "Cart/CompleteOrder/" + cartId + "/" + userId;
                var response = await _httpClient.PostAsync(url, null); // Assuming no request body for this API call

                if (!response.IsSuccessStatusCode)
                {
                    // If the request was not successful, return false
                    return false;
                }

                // If the request was successful, return true
                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, error handling, etc.)
                return false; // Return false in case of error
            }
        }
        public async Task<CustomerDtos1> GetUserById(string customerId)
        {
            try
            {
                // URL for retrieving shipper info based on cartId
                var url = _configuration["https:localAPI"] + "Admin/GetUserById/" + customerId;
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle the case where the API call was not successful
                    return null;
                }

                // Deserialize the response content into the CustomerDtos object
                var content = await response.Content.ReadAsStringAsync();
                var customerInfo = JsonConvert.DeserializeObject<ApiResponse<CustomerDtos1>>(content);
                return customerInfo.Data;
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, error handling, etc.)
                return null; // Return null in case of error
            }
        }
    }
}
