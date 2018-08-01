using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace YoulaApi.Models
{
    public abstract class AuthRequestUriAbstract
    {
        protected readonly Random _random = new Random(854667188);

        protected string Application = "Youla";

        protected string Service = "youla_registration";

        protected string Platform = "android";

        protected string Language = "ru_RU";

        protected string Key = "6e6746557930777a6b766d44726d7a35";

        public abstract string CreateUri();

        protected abstract string CreateSignature();

        protected string CtrateMD5(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                str = _random.Next().ToString();
            }
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            md5.Dispose();
            return sb.ToString().ToLower();
        }
    }
}
