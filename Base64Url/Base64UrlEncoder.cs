using System;
using System.Linq;

namespace Base64Url
{
    public static class Base64UrlEncoder
    {
        private const char padding = '=';

        /// <summary>
        /// Encode bytes with Base64Url
        /// </summary>
        /// <param name="data">The bytes to encode</param>
        /// <returns>Base64Url encoded <paramref name="data"/></returns>
        /// <exception cref="ArgumentNullException">When data is null or empty</exception>
        public static string Encode(byte[] data)
        {
            if (data is null || !data.Any())
                throw new ArgumentNullException(nameof(data), $"'{nameof(data)}' cannot be null or empty.");

            return Convert.ToBase64String(data)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd(padding);
        }

        private static string ToBase64(string base64Url)
        {
            return base64Url
                .Replace('-', '+')
                .Replace('_', '/')
                .PadRight(base64Url.Length + (4 - base64Url.Length % 4) % 4, padding);
        }

        /// <summary>
        /// Decode bytes from Base64Url
        /// </summary>
        /// <param name="encodedData">The Base64Url encoded string</param>
        /// <returns>Decoded bytes of <paramref name="encodedData"/></returns>
        /// <exception cref="ArgumentException">When encoded data is null or empty or is not a valid Base64Url encoded string</exception>
        public static byte[] Decode(string encodedData)
        {
            if (!Validate(encodedData, out _))
                throw new ArgumentException($"'{nameof(encodedData)}' is not valid Base64Url string.");

            return Convert.FromBase64String(ToBase64(encodedData));
        }

        /// <summary>
        /// Determine if <paramref name="data"/> is valid Base64Url encoded string
        /// </summary>
        /// <param name="data">The string to validate</param>
        /// <param name="bytesWritten">The number of bytes that were written</param>
        /// <returns>True if <paramref name="data"/> is valid Base64Url encoded string; otherwise, false</returns>
        public static bool Validate(string data, out int bytesWritten)
        {
            bytesWritten = 0;

            if (string.IsNullOrEmpty(data)) return false;

            var base64 = ToBase64(data);

            return Convert.TryFromBase64String(base64, stackalloc byte[base64.Length], out bytesWritten);
        }
    }
}