using System;
using System.Text;
using System.Text.RegularExpressions;

namespace LuaBridge.Core.Extensions
{
    public static class StringExtensions
    {
        private static readonly char[] base64padding = { '=' };

        /// <summary>
        /// Regex pattern to remove \r (carriage return)
        /// </summary>
        private static readonly string TextSanitizePattern = @"\r";

        public static string ToUrlSafeBase64(this string s)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s)).TrimEnd(base64padding).Replace('+', '-').Replace('/', '_');
        }

        public static string FromUrlSafeBase64(this string s)
        {
            string incoming = s.Replace('_', '/').Replace('-', '+');
            switch (s.Length % 4)
            {
                case 2:
                    incoming += "==";
                    break;
                case 3:
                    incoming += "=";
                    break;
            }

            byte[] bytes = Convert.FromBase64String(incoming);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Create a <link> rich text 
        /// </summary>
        /// <param name="value">the visible text</param>
        /// <param name="id">link id, to parse with textmeshpro</param>
        /// <param name="hexColorCode">color</param>
        /// <returns>embedded link</returns>
        public static string ToEmbeddedLink(this string value, string id, string hexColorCode)
        {
            return $"<link=\"{id}\"><b><color=#{hexColorCode}>{value}</color></b></link>";
        }

        /// <summary>
        /// Removes carriage return characters and leading and trailing whitespace
        /// </summary>
        /// <param name="str">string to sanitize</param>
        /// <returns>sanitized string</returns>
        public static string Sanitize(this string str)
        {
            return str.SanitizeCarriageReturn().TrimWhiteSpace();
        }

        /// <summary>
        /// Removes carriage return character from string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SanitizeCarriageReturn(this string input)
        {
            return string.IsNullOrEmpty(input) ? string.Empty : Regex.Replace(input, TextSanitizePattern, "");
        }

        /// <summary>
        /// Removes leading and trailing whitespace
        /// </summary>
        /// <param name="str">string to trim</param>
        /// <returns>trimmed string</returns>
        public static string TrimWhiteSpace(this string str)
        {
            char[] charsToTrim = {' ', '\t'};
            return str.Trim(charsToTrim);
        }

        public static string Replace(this string s, char[] separators, string newVal)
        {
            var temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(newVal, temp);
        }

        public static string CutAtIndex(this string value, int index)
        {
            return value[..index];
        }

        public static string ParseAndCut(this object obj, int index)
        {
            return obj.ToString().CutAtIndex(index);
        }
    }
}