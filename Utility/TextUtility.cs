using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtonFather.Utility
{
    public class TextUtility
    {
        public string RandomText(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string charsLower = chars.ToLower();
            string subMail = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return subMail;
        }
        /// <summary>
        /// Lấy ra random số theo chiều dài
        /// </summary>
        /// <returns></returns>
        public string RandomNumber(int length)
        {
            string chars = "0123456789";
            Random random = new Random();
            string suffix = new string(Enumerable.Repeat(chars, length)
               .Select(s => s[random.Next(s.Length)]).ToArray());

            return suffix;
        }

        public string RandomSerial(int length)
        {
            string chars = "abcdef1234567890";
            Random random = new Random();
            string suffix = new string(Enumerable.Repeat(chars, length)
               .Select(s => s[random.Next(s.Length)]).ToArray());

            return suffix;
        }
        /// <summary>
        /// Tạo mật khẩu random 8 kí tự không liên tiếp
        /// </summary>
        /// <returns></returns>
        public string GeneratePassword()
        {
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            string charsLower = chars.ToLower();
            string subMail = new string(Enumerable.Repeat(chars, random.Next(8, 10))
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return subMail;
        }
        public string GenerateName(int lenght)
        {
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            string charsLower = chars.ToLower();
            string subMail = new string(Enumerable.Repeat(chars, lenght)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return subMail;
        }
        static bool IsSequence(char a, char b, char c)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            string digits = "0123456789";
            string sequence = $"{a}{b}{c}";
            if (alphabet.Contains(sequence) || alphabet.Contains(new string(sequence.Reverse().ToArray())))
            {
                return true;
            }
            if (digits.Contains(sequence) || digits.Contains(new string(sequence.Reverse().ToArray())))
            {
                return true;
            }
            return false;
        }
    }
}
