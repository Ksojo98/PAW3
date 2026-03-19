using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PAW3.Architecture.Helpers;

public interface IFormatHelper
{
    // TEXT
    string ToTitleCase(string input, CultureInfo? culture = null);
    string Truncate(string input, int maxLength, string suffix = "...");
    string RemoveAccents(string input);
    string Slugify(string input);
    bool IsNullOrEmpty(string? input);

    // DATES
    string FormatDate(DateTime date, string format = "yyyy-MM-dd", CultureInfo? culture = null);
    string FormatDateLong(DateTime date, CultureInfo? culture = null);
    string TimeAgo(DateTime date);
    int CalculateAge(DateTime birthDate);
    string ToRelativeDate(DateTime date);

    // NUMBERS
    string FormatCurrency(decimal amount, string? culture = null);
    string FormatNumber(decimal number, int decimals = 2, string? culture = null);
    string FormatPercentage(double value, int decimals = 2);
    string AbbreviateNumber(long number); // 1K, 1M, etc.

    // GENERAL
    string FormatPhone(string phone);
    string Mask(string input, int visibleStart = 2, int visibleEnd = 2);
    string DefaultIfNull(string? input, string defaultValue = "-");

    // VALIDATION / PARSING
    bool TryParseDate(string input, out DateTime result);
    bool IsValidEmail(string email);
}

public class FormatHelper : IFormatHelper
{
    private readonly CultureInfo _defaultCulture;

    public FormatHelper()
    {
        _defaultCulture = CultureInfo.CurrentCulture;
    }

    // ---------------- TEXT ----------------
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

    // ---------------- DATES ----------------
    public string FormatDate(DateTime date, string format = "yyyy-MM-dd", CultureInfo? culture = null)
    {
        return date.ToString(format, culture ?? _defaultCulture);
    }

    public string FormatDateLong(DateTime date, CultureInfo? culture = null)
    {
        return date.ToString("D", culture ?? _defaultCulture);
    }

    public string TimeAgo(DateTime date)
    {
        var span = DateTime.Now - date;

        if (span.TotalSeconds < 60) return "just now";
        if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes} min ago";
        if (span.TotalHours < 24) return $"{(int)span.TotalHours} hrs ago";
        if (span.TotalDays < 30) return $"{(int)span.TotalDays} days ago";

        return FormatDate(date);
    }

    public int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
            age--;

        return age;
    }

    public string ToRelativeDate(DateTime date)
    {
        if (date.Date == DateTime.Today) return "Today";
        if (date.Date == DateTime.Today.AddDays(-1)) return "Yesterday";
        return FormatDate(date);
    }

    // ---------------- NUMBERS ----------------
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

    // ---------------- GENERAL ----------------
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

    // ---------------- VALIDATION ----------------
    public bool TryParseDate(string input, out DateTime result)
    {
        return DateTime.TryParse(input, out result);
    }

    public bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}

