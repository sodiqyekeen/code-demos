using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolutionApp
{
    static class StringExtensions
    {
        public static string MaskSensitiveJsonData(this string jsonString, string maskAttribute)
        {
            try
            {
                if (string.IsNullOrEmpty(jsonString?.Trim()) || string.IsNullOrEmpty(maskAttribute?.Trim())) return jsonString;

                var maskingAttributes = maskAttribute.Split('|');

                foreach (string key in maskingAttributes)
                {
                    string pattern = $"\"{key}\"[:]\\s(\\\"?.*\\\"?)";
                    var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    var distinctMatch = regex.Matches(jsonString).Select(m => m.Value).Distinct().ToList();
                    distinctMatch.ForEach(m =>
                    {
                        string replacementString = $"\"{key}\": \"" + m.Split(':').LastOrDefault().Mask();
                        jsonString = Regex.Replace(jsonString, m, replacementString);
                    });
                }

                return jsonString;
            }
            catch (Exception)
            {
                return jsonString;
            }
        }

        static string Mask(this string value)
        {
            //Implement masking logic here.
            return value;
        }

        static string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        static string GetIpAddress() =>
            Dns.GetHostEntry(Dns.GetHostName()).AddressList?.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString() ?? string.Empty;

        static string GenerateRandomNumber(int length)
        {
            var numbers = "0123456789".ToCharArray();
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[4 * length];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var token = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int index = BitConverter.ToUInt16(randomBytes, i * 4) % numbers.Length;
                token.Append(numbers[index]);
            }
            return token.ToString();
        }

        static string GeneratePassword()
        {
            var opts = new 
            {
                RequiredLength = 10,
                RequiredUniqueChars = 6,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] { "ABCDEFGHJKLMNOPQRSTUVWXYZ", "abcdefghijkmnopqrstuvwxyz", "0123456789", "!@#$%&()_-+." };
            var rand = new Random();
            var chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        public static string GetMaxNCharacters(this string value, int maxLength) =>
           value switch
           {
               null => string.Empty,
               string x when x.Trim().Length > maxLength => value.Substring(0, maxLength),
               _ => value.Trim()
           };

        public static bool IsNumeric(this string value)
        {
            return (long.TryParse(value.Trim(), out _));
        }

        public static string Between(this string value, string a, string b)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            int startIndex1 = value.IndexOf(a);
            int num = value.IndexOf(b, startIndex1);
            if (startIndex1 == -1 || num == -1)
                return string.Empty;
            int startIndex2 = startIndex1 + a.Length;
            return startIndex2 >= num ? string.Empty : value.Substring(startIndex2, num - startIndex2).Trim();
        }

        public static string Before(this string value, string str) =>
            value.IndexOf(str) > 1 ?
            value.Substring(0, value.IndexOf(str)).Trim() :
            value;

        public static string After(this string value, string str) =>
            value.IndexOf(str) > -1 ?
            value.Substring(value.IndexOf(str) + 1).Trim() :
            value;

    }
}
