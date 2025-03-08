using BLL.IService;
using BLL.Model;
using BLL.Model.Address;
using BLL.Model.Cart;
using BLL.Model.Customer;
using BLL.Model.Store;
using BLL.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BLL.Model.Collection;
using Microsoft.AspNetCore.Mvc;
using BLL.Model.Comment;
namespace BLL.Service
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminService> _logger;

        public AdminService(IConfiguration configuration, ILogger<AdminService> logger)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            _logger = logger;
        }
        public async Task ActivateStore(int storeId)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/ActivateStore/{storeId}";
                var response = await _httpClient.PostAsync(url, null);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to activate store with ID {storeId}. Status code: {response.StatusCode}");
                    throw new InvalidOperationException("Failed to activate store.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in ActivateStore: {ex.Message}");
                throw;
            }
        }

        public async Task<StoreDtos> AddStore(CreateStoreRequest createStoreRequest)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/AddStore";
                var response = await _httpClient.PostAsJsonAsync(url, createStoreRequest);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to add store. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var store = JsonConvert.DeserializeObject<StoreDtos>(content);
                return store;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in AddStore: {ex.Message}");
                throw;
            }
        }
        public async Task<CollectionDtos> GetCollectionById(int collectionID)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Collection/{collectionID}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to get collection. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var collection = JsonConvert.DeserializeObject<CollectionDtos>(content);

                return collection;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetCollectionById: {ex.Message}");
                throw;
            }
        }
        public async Task<List<CityDtos>> GetCities()
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Address/Cities";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve cities. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var cities = JsonConvert.DeserializeObject<ApiResponse<List<CityDtos>>>(content);
                return cities?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetCities: {ex.Message}");
                throw;
            }
        }

        public async Task<List<DistrictDtos>> GetDistrictsByCityId(int cityId)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Address/City/{cityId}/District";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve districts for city ID {cityId}. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var districts = JsonConvert.DeserializeObject<ApiResponse<List<DistrictDtos>>>(content);
                return districts?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetDistrictsByCityId: {ex.Message}");
                throw;
            }
        }

        public async Task<List<WardDtos>> GetWardsByDistrictId(int districtId)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Address/District/{districtId}/Wards";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve wards for district ID {districtId}. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var wards = JsonConvert.DeserializeObject<ApiResponse<List<WardDtos>>>(content);
                return wards?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetWardsByDistrictId: {ex.Message}");
                throw;
            }
        }

        public async Task AddUser(CreateCustomerRequest createCustomerRequest)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/AddUser";
                var response = await _httpClient.PostAsJsonAsync(url, createCustomerRequest);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to add user. Status code: {response.StatusCode}");
                    throw new InvalidOperationException("Failed to add user.");
                }

                var content = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<CustomerDtos>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in AddUser: {ex.Message}");
                throw;
            }
        }

        public async Task AssignRoleToUser(int storeId, string roleName, string loginName)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/AssignRoleToUser/{storeId}/{roleName}/{loginName}";
                var response = await _httpClient.PostAsync(url, null);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to assign role {roleName} to user {loginName} in store {storeId}. Status code: {response.StatusCode}");
                    throw new InvalidOperationException("Failed to assign role to user.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in AssignRoleToUser: {ex.Message}");
                throw;
            }
        }

        public async Task DeactivateStore(int storeId)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/DeactivateStore/{storeId}";
                var response = await _httpClient.PostAsync(url, null);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to deactivate store with ID {storeId}. Status code: {response.StatusCode}");
                    throw new InvalidOperationException("Failed to deactivate store.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in DeactivateStore: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteStore(int storeId)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/DeleteStore/{storeId}";
                var response = await _httpClient.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to delete store with ID {storeId}. Status code: {response.StatusCode}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in DeleteStore: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteUser(string customerId)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/DeleteUser/{customerId}";
                var response = await _httpClient.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to delete user with ID {customerId}. Status code: {response.StatusCode}");
                    throw new InvalidOperationException("Failed to delete user.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in DeleteUser: {ex.Message}");
                throw;
            }
        }

        public async Task<List<CartDtos>> GetAllOrders()
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetAllOrders";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve all orders. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<ApiResponse<List<CartDtos>>>(content);
                return orders.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetAllOrders: {ex.Message}");
                throw;
            }
        }
        public async Task<List<CartDtos>> GetAllOrdersToday()
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetAllOrdersToday";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve all orders. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<ApiResponse<List<CartDtos>>>(content);
                return orders.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetAllOrders: {ex.Message}");
                throw;
            }
        }

        public async Task<List<StoreDtos>> GetAllStores()
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetAllStores";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve all stores. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var stores = JsonConvert.DeserializeObject<ApiResponse<List<StoreDtos>>>(content);
                return stores.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetAllStores: {ex.Message}");
                throw;
            }
        }

        public async Task<List<CustomerDtos>> GetAllUsers()
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetAllUsers";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve all users. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<ApiResponse<List<CustomerDtos>>>(content);
                return users.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetAllUsers: {ex.Message}");
                throw;
            }
        }

        public  async Task<List<CartDtos>> GetOrdersByStoreId(int storeId)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetOrdersByStoreId/{storeId}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve orders for store with ID {storeId}. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<ApiResponse<List<CartDtos>>>(content);
                return orders.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetOrdersByStoreId: {ex.Message}");
                throw;
            }
        }
        public async Task<double> GetTotalRevenueForAllStoresToday()
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetTotalRevenueForAllStoresToday";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve total revenue for all stores. Status code: {response.StatusCode}");
                    return -1; // Return a default value to indicate failure
                }

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"API Response Content: {content}");

                // Deserialize JSON response to ApiResponse<double>
                var result = JsonConvert.DeserializeObject<ApiResponse<double>>(content);

                if (result == null || !result.IsSuccess)
                {
                    _logger.LogError($"API returned an error or failed to parse response. Message: {result?.Message}");
                    return -1;
                }

                return result.Data; // Return the revenue value
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetTotalRevenueForAllStoresToday: {ex.Message}");
                throw;
            }
        }
        public async Task<List<double>> GetTotalRevenueForLast8Weeks()
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetTotalRevenueForLast8Weeks";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve total revenue for the last 8 weeks. Status code: {response.StatusCode}");
                    return new List<double>(); // Trả về danh sách rỗng nếu có lỗi
                }

                var content = await response.Content.ReadAsStringAsync();
                var revenues = JsonConvert.DeserializeObject<List<double>>(content);
                return revenues;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetTotalRevenueForLast8Weeks: {ex.Message}");
                throw;
            }
        }
        public async Task<List<double>> GetTotalRevenueForLast12Months()
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetTotalRevenueForLast12Months";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve total revenue for the last 12 months. Status code: {response.StatusCode}");
                    return new List<double>(); // Trả về danh sách rỗng nếu có lỗi
                }

                var content = await response.Content.ReadAsStringAsync();
                var revenues = JsonConvert.DeserializeObject<List<double>>(content);
                return revenues;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetTotalRevenueForLast12Months: {ex.Message}");
                throw;
            }
        }

        public async Task<double> GetRevenueByStoreId(int storeId)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetRevenueByStoreId/{storeId}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve revenue for store with ID {storeId}. Status code: {response.StatusCode}");
                    return -1;
                }

                var content = await response.Content.ReadAsStringAsync();
                var revenue = JsonConvert.DeserializeObject<double>(content);
                return revenue;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetRevenueByStoreId: {ex.Message}");
                throw;
            }
        }

        public async Task<double> GetRevenueByStoreIdForDate(int storeId, DateTime date)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetRevenueByStoreIdForDate?storeId={storeId}&date={date.ToString("yyyy-MM-dd")}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve revenue for store {storeId} on date {date}. Status code: {response.StatusCode}");
                    return -1;
                }

                var content = await response.Content.ReadAsStringAsync();
                var revenue = JsonConvert.DeserializeObject<double>(content);
                return revenue;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetRevenueByStoreIdForDate: {ex.Message}");
                throw;
            }
        }

        public async Task<double> GetRevenueByStoreIdForMonth(int storeId, int year, int month)
        {
            try
            {
                // Validate input parameters
                if (storeId <= 0 || month < 1 || month > 12 || year < 1)
                {
                    throw new ArgumentException("Invalid input parameters.");
                }

                // Call the API endpoint to get the total revenue for the specified month, year, and store
                var url = $"{_configuration["https:localAPI"]}Admin/GetRevenueForMonth?storeID={storeId}&month={month}&year={year}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve revenue for store {storeId}, month {month}, year {year}. StatusCode: {response.StatusCode}");
                    return -1;
                }

                var content = await response.Content.ReadAsStringAsync();
                var revenue = JsonConvert.DeserializeObject<double>(content);
                return revenue;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetRevenueForMonth: {ex.Message}");
                throw;
            }
        }

        public async Task<double> GetRevenueByStoreIdForThisMonth(int storeId)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetRevenueByStoreIdForThisMonth/{storeId}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve revenue for store {storeId} for the current month. Status code: {response.StatusCode}");
                    return -1;
                }

                var content = await response.Content.ReadAsStringAsync();
                var revenue = JsonConvert.DeserializeObject<double>(content);
                return revenue;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetRevenueByStoreIdForThisMonth: {ex.Message}");
                throw;
            }
        }

        public async Task<double> GetRevenueByStoreIdForToday(int storeId)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetRevenueByStoreIdForToday/{storeId}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve revenue for store {storeId} for today. Status code: {response.StatusCode}");
                    return -1;
                }

                var content = await response.Content.ReadAsStringAsync();
                var revenue = JsonConvert.DeserializeObject<double>(content);
                return revenue;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetRevenueByStoreIdForToday: {ex.Message}");
                throw;
            }
        }

        public async Task<double> GetRevenueByStoreIdForYear(int storeId, int year)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetRevenueByStoreIdForYear?storeId={storeId}&year={year}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve revenue for store {storeId} for year {year}. Status code: {response.StatusCode}");
                    return -1;
                }

                var content = await response.Content.ReadAsStringAsync();
                var revenue = JsonConvert.DeserializeObject<double>(content);
                return revenue;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetRevenueByStoreIdForYear: {ex.Message}");
                throw;
            }
        }

        public async Task<List<StoreDtos>> GetStoresByStatus(int status)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/GetStoresByStatus/{status}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve stores with status {status}. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var stores = JsonConvert.DeserializeObject<ApiResponse<List<StoreDtos>>>(content);
                return stores.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetStoresByStatus: {ex.Message}");
                throw;
            }
        }

        public async Task<CustomerDtos> GetUserById(string customerId)
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
                var customerInfo = JsonConvert.DeserializeObject<ApiResponse<CustomerDtos>>(content);
                return customerInfo.Data;
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, error handling, etc.)
                return null; // Return null in case of error
            }
        }

        public async Task<List<string>> GetUserRoles(int storeId, string username)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/get-user-roles?storeId={storeId}&username={username}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to get roles for user {username} in store {storeId}. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<List<string>>(content);
                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetUserRoles: {ex.Message}");
                throw;
            }

        }

        public async Task<bool> IsUserGlobalAdmin(int storeId, string loginName)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/IsUserGlobalAdmin/{storeId}/{loginName}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to check if user {loginName} is a global admin in store {storeId}. Status code: {response.StatusCode}");
                    return false;
                }

                var content = await response.Content.ReadAsStringAsync();
                var isGlobalAdmin = JsonConvert.DeserializeObject<bool>(content);
                return isGlobalAdmin;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in IsUserGlobalAdmin: {ex.Message}");
                throw;
            }
        }

        public async Task LockStore(int storeId)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/LockStore/{storeId}";
                var response = await _httpClient.PostAsync(url, null);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to lock store with ID {storeId}. Status code: {response.StatusCode}");
                    throw new InvalidOperationException("Failed to lock store.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in LockStore: {ex.Message}");
                throw;
            }
        }

        public async Task RemoveRoleFromUser(int storeId, string roleName, string loginName)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/RemoveRoleFromUser/{storeId}/{roleName}/{loginName}";
                var response = await _httpClient.PostAsync(url, null);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to remove role {roleName} from user {loginName} in store {storeId}. Status code: {response.StatusCode}");
                    throw new InvalidOperationException("Failed to remove role from user.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in RemoveRoleFromUser: {ex.Message}");
                throw;
            }
        }

        public async Task UnlockStore(int storeId)
        {
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/UnlockStore/{storeId}";
                var response = await _httpClient.PostAsync(url, null);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to unlock store with ID {storeId}. Status code: {response.StatusCode}");
                    throw new InvalidOperationException("Failed to unlock store.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in UnlockStore: {ex.Message}");
                throw;
            }
        }

        public async Task<List<CommentDtos>> GetAllCommentStores()
        {
            //try
            //{
            //    // Tạo URL API từ cấu hình
            //    var url = $"{_configuration["https:localAPI"]}Admin/get-all-comments";

            //    // Gửi yêu cầu POST đến API
            //    var response = await _httpClient.PostAsync(url, null);

            //    // Kiểm tra trạng thái của phản hồi từ API
            //    if (!response.IsSuccessStatusCode)
            //    {
            //        _logger.LogError($"Fail GetAllComments. Status code: {response.StatusCode}");
            //        throw new InvalidOperationException("Failed to unlock store.");
            //    }

            //    // Lấy dữ liệu JSON từ phản hồi
            //    var jsonResponse = await response.Content.ReadAsStringAsync();

            //    // Chuyển đổi dữ liệu JSON thành đối tượng List<CommentDtos>
            //    var commentDtos = JsonConvert.DeserializeObject<List<CommentDtos>>(jsonResponse);

            //    // Trả về danh sách CommentDtos
            //    return commentDtos;
            //}
            //catch (Exception ex)
            //{
            //    // Ghi log lỗi nếu có exception xảy ra
            //    _logger.LogError($"Exception in GetAllCommentStores: {ex.Message}");
            //    throw;
            //}
            try
            {
                var url = $"{_configuration["https:localAPI"]}Admin/get-all-comments";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve all comments. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<List<CommentDtos>>(content); // Chỉ cần List<CommentDtos> nếu dữ liệu trả về là mảng.

                // Trả về danh sách comments nếu thành công
                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred while retrieving all comments: {ex.Message}\n{ex.StackTrace}");
                throw; // Ném lại exception để có thể xử lý ở nơi khác nếu cần
            }


        }
        public async Task<bool> DeleteComment(int commentId)
        {
            try
            {
                // Construct the URL for the delete API
                var url = $"{_configuration["https:localAPI"]}Comment/{commentId}";

                // Send a DELETE request to the API
                var response = await _httpClient.DeleteAsync(url);

                // Check if the response is successful
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to delete comment. Status code: {response.StatusCode}");
                    return false; // Indicate failure
                }

                _logger.LogInformation($"Comment with ID {commentId} deleted successfully.");
                return true; // Indicate success
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the request
                _logger.LogError($"Exception occurred while deleting comment: {ex.Message}\n{ex.StackTrace}");
                throw; // Re-throw the exception to handle it in a higher layer if needed
            }
        }

    }
}
