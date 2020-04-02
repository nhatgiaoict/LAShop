using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LACoreApp.Utilities.Helpers
{
    public static class TextHelper
    {
        public static string ToUnsignString(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            input = input.Replace(".", "-");
            input = input.Replace(" ", "-");
            input = input.Replace(",", "-");
            input = input.Replace(";", "-");
            input = input.Replace(":", "-");
            input = input.Replace("  ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            while (str2.Contains("--"))
            {
                str2 = str2.Replace("--", "-").ToLower();
            }
            return str2;
        }
        public static string ToString(decimal number)
        {
            string s = number.ToString("#");
            string[] numberWords = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] layer = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, unit, dozen, hundred;
            string str = " ";
            bool booAm = false;
            decimal decS = 0;
            //Tung addnew
            try
            {
                decS = Convert.ToDecimal(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = numberWords[0] + str;
            else
            {
                j = 0;
                while (i > 0)
                {
                    unit = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        dozen = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        dozen = -1;
                    i--;
                    if (i > 0)
                        hundred = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        hundred = -1;
                    i--;
                    if ((unit > 0) || (dozen > 0) || (hundred > 0) || (j == 3))
                        str = layer[j] + str;
                    j++;
                    if (j > 3) j = 1;
                    if ((unit == 1) && (dozen > 1))
                        str = "một " + str;
                    else
                    {
                        if ((unit == 5) && (dozen > 0))
                            str = "lăm " + str;
                        else if (unit > 0)
                            str = numberWords[unit] + " " + str;
                    }
                    if (dozen < 0)
                        break;
                    else
                    {
                        if ((dozen == 0) && (unit > 0)) str = "lẻ " + str;
                        if (dozen == 1) str = "mười " + str;
                        if (dozen > 1) str = numberWords[dozen] + " mươi " + str;
                    }
                    if (hundred < 0) break;
                    else
                    {
                        if ((hundred > 0) || (dozen > 0) || (unit > 0)) str = numberWords[hundred] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm) str = "Âm " + str;
            return str + "đồng chẵn";
        }

        public static string InnerTruncate(this string value, int maxLength)
        {
            // If there is no need to truncate then
            // return what we were given.
            if (string.IsNullOrEmpty(value)
                    || value.Length <= maxLength)
            {
                return value;
            } // end if

            // Figure out how many characters would be in 
            // each  half if we were to have
            // exactly the same length string on either side 
            // of the elipsis.
            int charsInEachHalf = (maxLength - 3) / 2;

            // Get the string to the right of the elipsis 
            // and then trim the beginning.  There is no
            // need to have a space immediately following 
            // the elipsis.
            string right = value.Substring(
                value.Length - charsInEachHalf, charsInEachHalf)
                .TrimStart();

            // Get the string to the left of the elipsis.
            // We don't use "charsInEachHalf " here
            // because we may be able to take more characters
            // than that if "right" was trimmed.
            string left = value.Substring(
                0, (maxLength - 3) - right.Length)
                .TrimEnd();

            // Concatenate and return the result.
            return string.Format("{0}...{1}", left, right);
        } 

        public static string ToStringLimit(this string text, int maxLength, bool showEllipsis = true)
        {
            if (maxLength < 0) throw new ArgumentOutOfRangeException("maxLength", "Value must not be negative");
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            var n = text.Length;
            var ellipsis = showEllipsis ? "..." : string.Empty;
            var minLength = ellipsis.Length;
            maxLength = Math.Max(minLength, maxLength);
            return n > maxLength ? text.Substring(0, Math.Min(maxLength - minLength, n)) + ellipsis : text;
        }

        public static int ToInt(this string s)
        {
            var returnInt = 0;

            try
            {
                returnInt = int.Parse(s);
            }
            catch (Exception)
            {
                //throw ex;
            }

            return returnInt;
        }
    }
}
