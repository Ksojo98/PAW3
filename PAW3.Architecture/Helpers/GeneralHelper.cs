using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PAW3.Architecture.Helpers
{
    public interface IGeneralHelper
    {
        string FormatPhone(string phone);
        string Mask(string input, int visibleStart = 2, int visibleEnd = 2);
        string DefaultIfNull(string? input, string defaultValue = "-");
    }

    public class GeneralHelper : IGeneralHelper
    {
        private readonly CultureInfo _defaultCulture;

        public GeneralHelper()
        {
            _defaultCulture = CultureInfo.CurrentCulture;
        }
        public string FormatPhone(string phone)
        {
            return Regex.Replace(phone, @"(\d{4})(\d{4})", "$1-$2");
        }

        public string Mask(string input, int visibleStart = 2, int visibleEnd = 2)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= visibleStart + visibleEnd)
                return input;

            var masked = new string('*', input.Length - visibleStart - visibleEnd);
            return input.Substring(0, visibleStart) + masked + input.Substring(input.Length - visibleEnd);
        }

        public string DefaultIfNull(string? input, string defaultValue = "-")
        {
            return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
        }
    }
}
