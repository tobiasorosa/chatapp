using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chatapp.Core.Extensions
{
    public static class StringExtension
    {
        public static decimal ToDecimal(this string value)
        {
            decimal.TryParse(value, out decimal result);

            return result;
        }
        public static decimal ToDecimalAmericanToReal(this string value)
        {
            return ToDecimal(value.Replace(".", ","));
        }
        public static decimal ToDecimalAmerican(this string value)
        {
            var culture = new CultureInfo("en-US");

            decimal.TryParse(value, NumberStyles.Any, culture, out decimal result);

            return result;
        }
        public static int ToInt(this string value)
        {
            int.TryParse(value, out int result);

            return result;
        }
        public static long ToLong(this string value)
        {
            long.TryParse(value, out long result);

            return result;
        }
        public static DateTime ToDateTime(this string input)
        {
            DateTime.TryParse(input, out DateTime result);

            return result;
        }

        public static string FitStringLength(this string sringToBeFit, int maxLength, char fitChar)
            => sringToBeFit.Length > maxLength ? sringToBeFit.Substring(0, maxLength) : sringToBeFit.PadLeft(maxLength, fitChar);

        public static DateTime TryParseToDateTime(this string dayMonthYear)
        {
            if (dayMonthYear.Length < 8 || dayMonthYear.Length > 10)
            {
                return new DateTime();
            }

            if (dayMonthYear.Length != 10)
            {
                dayMonthYear = dayMonthYear.Insert(2, "/").Insert(5, "/");
            }

            DateTime.TryParse(dayMonthYear, out DateTime result);

            return result;
        }

        public static bool EqualsIgnoreCase(this string left, string right) => left.Equals(right, StringComparison.OrdinalIgnoreCase);

        public static bool ContainsIgnoreCase(this string left, string right) => left.Contains(right, StringComparison.OrdinalIgnoreCase);

        public static string OnlyNumbers(this string value) => string.Join("", value.ToCharArray().Where(char.IsDigit));

        public static IEnumerable<T> SplitGeneric<T>(this string value, string separator) where T : IConvertible
        {
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            return value.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                .Select(str => (T)Convert.ChangeType(str.Trim(), typeof(T)));
        }

        public static string ReplaceAccents(this string value)
        {
            string withAccents = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string withoutAccents = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < withAccents.Length; i++)
            {
                value = value.Replace(withAccents[i].ToString(), withoutAccents[i].ToString());
            }

            return value;
        }

        public static string RemoveCharacters(this string str, string character)
        {
            for (int i = 0; i < character.Length; i++)
            {
                str = str.Replace(character[i].ToString(), string.Empty);
            }

            return str;
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
        }

        public static string SubstringWithTrim(this string value, int startIndex, int length)
        {
            return value.Substring(startIndex, length).Trim();
        }

        public static bool ToBool(this string str)
        {
            return !str.ToUpper().Contains("N");
        }

        public static string PadLeftWithSubstring(this string valor, string preenchimento, int tamanho)
        {
            return valor.PadLeft(tamanho, Convert.ToChar(preenchimento)).Substring(0, tamanho);
        }

        public static string PadRightWithSubstring(this string valor, string preenchimento, int tamanho)
        {
            return valor.PadRight(tamanho, Convert.ToChar(preenchimento)).Substring(0, tamanho);
        }

        public static string PadRightWithSubstring(this string valor, int tamanho)
        {
            return valor.PadRight(tamanho).Substring(0, tamanho);
        }

        public static bool IsEmptyOrWhiteSpace(this string value) =>
            value.All(char.IsWhiteSpace);

        public static bool TryParseToEnum<T>(this string description, out T result)
        {
            var fields = typeof(T).GetFields();

            foreach (var field in fields)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0 && attributes[0].Description == description)
                {
                    result = (T)Enum.Parse(typeof(T), field.Name);

                    return true;
                }
            }

            result = default;

            return false;
        }

        public static string ToSnakeCaseV2(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public static string ToCamelCase(this string input)
        {
            var words = input.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);
            var leadWord = Regex.Replace(words[0], @"([A-Z])([A-Z]+|[a-z0-9]+)($|[A-Z]\w*)",
                m =>
                {
                    return m.Groups[1].Value.ToLower() + m.Groups[2].Value.ToLower() + m.Groups[3].Value;
                });
            var tailWords = words.Skip(1)
                .Select(word => char.ToUpper(word[0]) + word.Substring(1))
                .ToArray();
            return $"{leadWord}{string.Join(string.Empty, tailWords)}";
        }

        public static string ToPascalCase(this string input)
        {
            Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            Regex whiteSpace = new Regex(@"(?<=\s)");
            Regex startsWithLowerCaseChar = new Regex("^[a-z]");
            Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(input, "_"), string.Empty)
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

            return string.Concat(pascalCase);
        }

        public static string FormatPhoneNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return input.Length == 9 ?
                input.ToLong().ToString("####-####") :
                input.ToLong().ToString("#####-####");
        }

        public static byte[] ToUtf8(this string value)
        {
            return string.IsNullOrEmpty(value) ? default : Encoding.UTF8.GetBytes(value);
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string FormatYearMonth(this string value)
        {
            if (value.Length == 6)
            {
                var formattedDate = new StringBuilder(value);
                formattedDate.Insert(4, '/');
                return formattedDate.ToString();
            }
            else
            {
                return value;
            }
        }
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}
