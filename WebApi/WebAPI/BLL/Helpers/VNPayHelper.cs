using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // Để sử dụng HttpContex

namespace BLL.Helpers
{
    public static class VNPayHelper
    {
        public static string CreateSecureHash(string secretKey, string data)
        {
            // Chuyển đổi hash secret sang byte
            //using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey)))
            //{
            //    var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            //    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            //}
            var hash = new StringBuilder();
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2")); // Chuyển sang chuỗi hex
                }
            }
            return hash.ToString();
        }
        public static string BuildQueryString(SortedList<string, string> parameters)
        {
            var queryBuilder = new StringBuilder();
            
            foreach (var param in parameters.OrderBy(x => x.Key))
            {
                if (!string.IsNullOrEmpty(param.Value))
                {
                    queryBuilder.Append($"{param.Key}={param.Value}&");
                }
            }

            // Loại bỏ ký tự & cuối cùng
            return queryBuilder.ToString().TrimEnd('&');
        }
    }
}
