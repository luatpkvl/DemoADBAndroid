using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WhatsappAccount
{
    public static class StringExtension
    {
        /// <summary>
        /// xoas ky tu thua sau 250 ky tu
        /// created by: ltluat 04.06.2023
        /// </summary>
        /// <param name="pThis"></param>
        /// <param name="pLength"></param>
        /// <returns></returns>
        public static string Truncate(this string pThis, int pLength)
        {
            if (string.IsNullOrEmpty(pThis))
                return pThis;

            if (0 >= pLength)
                return string.Empty;

            var lTruncatedString = pThis;
            const string lEllipses = @"…";

            if (pThis.Length > pLength)
            {
                var lSubstringLength = Math.Max(pLength - lEllipses.Length, 0);
                lTruncatedString = pThis.Substring(0, lSubstringLength) + lEllipses;
                if (lTruncatedString.Length > pLength)
                    lTruncatedString = lTruncatedString.Substring(0, pLength);
            }

            return lTruncatedString;
        }
        /// <summary>
        /// Chuyển chữ sang không dấu
        /// created by: ltluat 04.06.2023
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ConvertToUnSign2(this string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }
        /// <summary>
        /// Lấy số từ text
        /// </summary>
        /// <param name="numberInput"></param>
        /// <returns></returns>
        public static string GetNumberCodeFromText(this string numberInput)
        {
            string pattern = @"\d{6}";
            MatchCollection matches = Regex.Matches(numberInput, pattern);

            if (matches.Count > 0)
            {
                return matches[0].Value;
            }
            return string.Empty;
        }
        /// <summary>
        /// Lấy ra định dạng số US từ chuỗi
        /// </summary>
        /// <param name="numberInput"></param>
        /// <returns></returns>
        public static string GetUsPhoneNumberFromText(this string numberInput)
        {
            string pattern = @"\d{10}";
            MatchCollection matches = Regex.Matches(numberInput, pattern);

            if (matches.Count > 0)
            {
                return matches[0].Value;
            }
            return string.Empty;
        }
    }
}
