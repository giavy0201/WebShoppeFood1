using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Helpers
{
    public class VnPayLibrary
    {
        public const string VERSION = "2.1.0";
        private SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
        private SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData[key] = value;
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData[key] = value;
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var value) ? value : string.Empty;
        }

        #region Request

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            var data = new StringBuilder();
            foreach (var kv in _requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            var queryString = data.ToString();
            if (queryString.Length > 0)
            {
                queryString = queryString.Remove(queryString.Length - 1); // Bỏ ký tự '&' cuối cùng
            }

            var signData = queryString;
            var vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);
            return $"{baseUrl}?{queryString}&vnp_SecureHash={vnp_SecureHash}";
        }

     

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var rspRaw = GetResponseData();
            var myChecksum = Utils.HmacSHA512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetResponseData()
        {
            var data = new StringBuilder();

            // Bỏ các trường không cần trong chữ ký
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }
            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }

            foreach (var kv in _responseData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1); // Bỏ ký tự '&' cuối cùng
            }
            return data.ToString();
        }

        #endregion
    }

    public static class Utils
    {
        public static string HmacSHA512(string key, string inputData)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
            return string.Concat(hashValue.Select(b => b.ToString("x2")));
        }

        public static string GetIpAddress(HttpContext httpContext)
        {
            try
            {
                // Lấy IP từ header "X-Forwarded-For" nếu có
                var ipAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

                // Nếu không có, sử dụng IP từ kết nối hiện tại
                if (string.IsNullOrEmpty(ipAddress) || ipAddress.ToLower() == "unknown")
                {
                    ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
                }

                // Xác minh địa chỉ IP
                if (!string.IsNullOrEmpty(ipAddress) && ipAddress.Length > 45)
                {
                    ipAddress = null; // Địa chỉ IP không hợp lệ
                }

                return ipAddress ?? "Invalid IP";
            }
            catch (Exception ex)
            {
                return $"Invalid IP: {ex.Message}";
            }
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            var compareInfo = CompareInfo.GetCompareInfo("en-US");
            return compareInfo.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
