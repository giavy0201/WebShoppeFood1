using BLL.IService;
using BLL.Model;
using BLL.Model.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BLL.Service
{
    public class CartService : ICartService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CartService> _logger;

        public CartService(IConfiguration configuration, ILogger<CartService> logger)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<CartDtos> GetCartByID(int CartID)
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
                var cart = JsonConvert.DeserializeObject<ApiResponse<CartDtos>>(content);
                return cart.Data;
            }
        }

        public async Task<double> GetRevenueForMonth(int storeID, int month, int year)
        {
            try
            {
                // Validate input parameters
                if (storeID <= 0 || month < 1 || month > 12 || year < 1)
                {
                    throw new ArgumentException("Invalid input parameters.");
                }

                // Call the API endpoint to get the total revenue for the specified month, year, and store
                var url = $"{_configuration["https:localAPI"]}TotalRevenueForMonth?storeID={storeID}&month={month}&year={year}";
                var response = await _httpClient.GetAsync(url);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    double revenue = JsonConvert.DeserializeObject<double>(content);
                    return revenue;
                }
                else
                {
                    // Log an error if the request failed
                    _logger.LogError($"Error getting revenue for store {storeID}, month {month}, year {year}. StatusCode: {response.StatusCode}");
                    return -1; // Or any appropriate default value
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur
                _logger.LogError($"Exception in GetRevenueForMonth: {ex.Message}");
                throw;
            }
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


    }

}
