using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CryptonatorApi.Extantions
{
    internal static class StringExtantions
    {
        /// <summary>
        /// Check string null or white spaces
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpaces(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// Convert string to sha1
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StringSha1(this string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        /// <summary>
        /// Sign transaction
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> SignPostData(this IEnumerable<KeyValuePair<string, string>> inputData)
        {
            var forHash = string.Join("&", inputData.Select(pair => pair.Value));
            var hashed = forHash.StringSha1();
            List<KeyValuePair<string, string>> resultPostData = new List<KeyValuePair<string, string>>(inputData.Take(inputData.Count() - 1));
            resultPostData.Add(new KeyValuePair<string, string>("secret_hash", hashed));
            return resultPostData;
        }


    }
}