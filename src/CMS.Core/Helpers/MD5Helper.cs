using System;
using System.Security.Cryptography;
using System.Text;

namespace CMS.Core.Helpers
{
    public class MD5Helper
    {
        public static string MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                var hash = BitConverter.ToString(result).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}
