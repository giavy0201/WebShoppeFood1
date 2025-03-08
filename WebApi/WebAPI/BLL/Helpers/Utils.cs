using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Helpers
{
    public static class Utilss
    {
        public static string GenerateSignature(string rawHash, string secretKey)
        {
            // You can use HMACSHA256 for signature generation
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawHash));
                return BitConverter.ToString(hash).Replace("-", "").ToLower(); // Return signature as a lowercase hex string
            }
        }
    }
}
