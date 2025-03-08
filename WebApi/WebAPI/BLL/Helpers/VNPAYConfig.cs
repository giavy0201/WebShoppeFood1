//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Microsoft.Extensions.Primitives;
//using NHibernate.Engine;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace BLL.Helpers
//{
//    public class VNPAYConfig
//    {

//        private readonly VnPayVariable vnPayVariable;

//        /**
//         * Hashes all fields in the provided dictionary using HMAC SHA-512.
//         *
//         * @param fields The dictionary of fields to hash.
//         * @return The HMAC SHA-512 hash of the fields.
//         */
//        public static string HashAllFields(Dictionary<string, string> fields)
//        {
//            // Sort field names
//            var fieldNames = fields.Keys.OrderBy(key => key).ToList();

//            // Build the raw data string
//            StringBuilder sb = new StringBuilder();
//            for (int i = 0; i < fieldNames.Count; i++)
//            {
//                string fieldName = fieldNames[i];
//                string fieldValue = fields[fieldName];
//                if (!string.IsNullOrEmpty(fieldValue))
//                {
//                    sb.Append(fieldName).Append("=").Append(fieldValue);
//                    if (i < fieldNames.Count - 1)
//                    {
//                        sb.Append("&");
//                    }
//                }
//            }

//            // Hash the resulting string with HMAC SHA-512
//            return HmacSHA512(VnPayVariable, sb.ToString());
//        }

//        /**
//         * Creates an HMAC SHA-512 hash for the given data using the provided key.
//         *
//         * @param key  The secret key.
//         * @param data The data to hash.
//         * @return The resulting HMAC SHA-512 hash in hexadecimal format.
//         */
//        public static string HmacSHA512(string key, string data)
//        {
//            try
//            {
//                if (key == null || data == null)
//                {
//                    throw new ArgumentException("Key and data must not be null.");
//                }

//                // Initialize HMAC SHA-512
//                using (var hmac512 = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
//                {
//                    // Perform hashing
//                    byte[] result = hmac512.ComputeHash(Encoding.UTF8.GetBytes(data));
//                    StringBuilder sb = new StringBuilder(result.Length * 2);
//                    foreach (byte b in result)
//                    {
//                        sb.AppendFormat("{0:x2}", b);
//                    }
//                    return sb.ToString();
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error while generating HMAC SHA-512 hash: " + ex.Message, ex);
//            }
//        }

//        /**
//         * Retrieves the IP address of the client from the HTTP request.
//         *
//         * @param request The HTTP request.
//         * @return The client's IP address.
//         */
//        public static string GetIpAddress(htt)
//        {
//            try
//            {
//                string ipAddress = request.Headers["X-FORWARDED-FOR"];
//                if (string.IsNullOrEmpty(ipAddress))
//                {
//                    ipAddress = request.UserHostAddress;
//                }
//                return ipAddress;
//            }
//            catch (Exception e)
//            {
//                return "Invalid IP: " + e.Message;
//            }
//        }

//        /**
//         * Generates a random numeric string of the specified length.
//         *
//         * @param length The desired length of the random number.
//         * @return A random numeric string.
//         */
//        public static string GetRandomNumber(int length)
//        {
//            Random random = new Random();
//            StringBuilder sb = new StringBuilder(length);
//            for (int i = 0; i < length; i++)
//            {
//                sb.Append(random.Next(10)); // Generate a digit (0-9)
//            }
//            return sb.ToString();
//        }
//    }
//}
