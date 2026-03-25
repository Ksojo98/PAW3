using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PAW3.Architecture.Helpers
{
    public interface ITextHelper
    {
        string ToTitleCase(string input, CultureInfo? culture = null);
        string Truncate(string input, int maxLength, string suffix = "...");
        string RemoveAccents(string input);
        string Slugify(string input);
        bool IsNullOrEmpty(string? input);
    }
   
    public class TextHelper : ITextHelper
    {
        private readonly CultureInfo _defaultCulture;
        public TextHelper()
        {
            _defaultCulture = CultureInfo.CurrentCulture;
        }
        public string ToTitleCase(string input, CultureInfo? culture = null)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            var textInfo = (culture ?? _defaultCulture).TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        public string Truncate(string input, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(input) || input.Length <= maxLength) return input;
            return input.Substring(0, maxLength) + suffix;
        }

        public string RemoveAccents(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public string Slugify(string input)
        {
            input = RemoveAccents(input).ToLower();
            input = Regex.Replace(input, @"[^a-z0-9\s-]", "");
            input = Regex.Replace(input, @"\s+", "-").Trim('-');
            return input;
        }

        public bool IsNullOrEmpty(string? input) => string.IsNullOrEmpty(input);
    }
}
