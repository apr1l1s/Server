using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server.Hashing
{
    static class HashHelper
    {
        private static Random _random = new Random(Environment.TickCount);
        public static string generateSalt()
        {
            string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            StringBuilder builder = new StringBuilder(10);

            for (int i = 0; i < 10; ++i)
                builder.Append(chars[_random.Next(chars.Length)]);

            return builder.ToString();
        }
        public static string HashString(string s)
        {
            using (var hasher = new SHA256Managed())
            {
                var unhashed = Encoding.UTF8.GetBytes(s);
                var hashedbutes = hasher.ComputeHash(unhashed);
                var hashedPassword = Convert.ToBase64String(hashedbutes);
                return hashedPassword;
            }
        }
        public static string UnhashString(string s)
        {
            using (var hasher = new SHA256Managed())
            {
                var unhashedbytes = Convert.FromBase64String(s);
                var password = Encoding.UTF8.GetString(unhashedbytes);
                return password;
            }
        }
    }
}
