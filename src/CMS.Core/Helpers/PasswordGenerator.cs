using CMS.Core.Extensions;
using System;
using System.Linq;

namespace CMS.Core.Helpers
{
    public static class PasswordGenerator
    {
        public static string GeneratePassword(string prefix = null)
        {
            return prefix.UseFallback("CMS@") + Guid.NewGuid().ToString("N").Substring(0, 6) + "!@#$";
        }

        public static string GenerateShortPassword()
        {
            var generator = new Random();
            var r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                r = GenerateShortPassword();
            }
            return r;
        }
    }
}
