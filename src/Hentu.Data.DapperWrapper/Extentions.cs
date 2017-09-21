using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hentu.Data.DapperWrapper
{
    public static class Extentions
    {
        public static string MD5<T>(this T id)
        {
            using (MD5CryptoServiceProvider md5csp = new MD5CryptoServiceProvider())
            {
                var buffer = md5csp.ComputeHash(Encoding.UTF8.GetBytes(id?.ToString() ?? string.Empty));
                return System.BitConverter.ToString(buffer).Replace("-", "").ToLower();
            }
        }
    }
}
