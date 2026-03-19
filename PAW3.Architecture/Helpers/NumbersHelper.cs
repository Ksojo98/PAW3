using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAW3.Architecture.Helpers
{
    public interface INumbersHelper 
    {
        string FormatCurrency(decimal amount, string? culture = null);
        string FormatNumber(decimal number, int decimals = 2, string? culture = null);
        string FormatPercentage(double value, int decimals = 2);
        string AbbreviateNumber(long number); // 1K, 1M, etc.
    }

    public class NumbersHelper : INumbersHelper
    {
        private readonly CultureInfo _defaultCulture;

        public NumbersHelper()
        {
            _defaultCulture = CultureInfo.CurrentCulture;
        }
        public string FormatCurrency(decimal amount, string? culture = null)
        {
            var ci = culture != null ? new CultureInfo(culture) : _defaultCulture;
            return string.Format(ci, "{0:C}", amount);
        }

        public string FormatNumber(decimal number, int decimals = 2, string? culture = null)
        {
            var ci = culture != null ? new CultureInfo(culture) : _defaultCulture;
            return number.ToString($"N{decimals}", ci);
        }

        public string FormatPercentage(double value, int decimals = 2)
        {
            return value.ToString($"P{decimals}");
        }

        public string AbbreviateNumber(long number)
        {
            if (number >= 1_000_000)
                return $"{number / 1_000_000.0:0.#}M";
            if (number >= 1_000)
                return $"{number / 1_000.0:0.#}K";
            return number.ToString();
        }
    }
}
