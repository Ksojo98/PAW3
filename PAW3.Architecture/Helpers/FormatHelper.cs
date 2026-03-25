using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PAW3.Architecture.Helpers;

public class FormatHelper : IDatesHelper, ITextHelper, IValidationHelper, IGeneralHelper, INumbersHelper 
{
    private readonly IDatesHelper _datesHelper;
    private readonly ITextHelper _textHelper;
    private readonly IGeneralHelper _generalHelper;
    private readonly INumbersHelper _numbersHelper;
    private readonly IValidationHelper _validationHelper;

    public FormatHelper(
         IDatesHelper datesHelper,
         ITextHelper textHelper,
         IValidationHelper validationHelper,
         IGeneralHelper generalHelper,
         INumbersHelper numbersHelper)
    {
        _datesHelper = datesHelper;
        _textHelper = textHelper;
        _validationHelper = validationHelper;
        _generalHelper = generalHelper;
        _numbersHelper = numbersHelper;
    }

    public string FormatDate(DateTime date, string format = "yyyy-MM-dd", CultureInfo? culture = null)
            => _datesHelper.FormatDate(date, format, culture);

    public string FormatDateLong(DateTime date, CultureInfo? culture = null)
        => _datesHelper.FormatDateLong(date, culture);

    public string TimeAgo(DateTime date)
        => _datesHelper.TimeAgo(date);

    public int CalculateAge(DateTime birthDate)
        => _datesHelper.CalculateAge(birthDate);

    public string ToRelativeDate(DateTime date)
        => _datesHelper.ToRelativeDate(date);

    public string ToTitleCase(string input, CultureInfo? culture = null)
        => _textHelper.ToTitleCase(input, culture);

    public string Truncate(string input, int maxLength, string suffix = "...")
        => _textHelper.Truncate(input, maxLength, suffix);
    public string RemoveAccents(string input)
        => _textHelper.RemoveAccents(input);
    public string Slugify(string input)
        => _textHelper.Slugify(input);
    public bool IsNullOrEmpty(string? input)
        => _textHelper.IsNullOrEmpty(input);
    public bool TryParseDate(string input, out DateTime result)
        => _validationHelper.TryParseDate(input, out result);
    public bool IsValidEmail(string email)
        => _validationHelper.IsValidEmail(email);
    public string FormatPhone(string phone)
        => _generalHelper.FormatPhone(phone);
    public string Mask(string input, int visibleStart = 2, int visibleEnd = 2)
        => _generalHelper.Mask(input, visibleStart, visibleEnd);
    public string DefaultIfNull(string? input, string defaultValue = "-")
        => _generalHelper.DefaultIfNull(input, defaultValue);
    public string FormatCurrency(decimal amount, string? culture = null)
        => _numbersHelper.FormatCurrency(amount, culture);
     public string FormatNumber(decimal number, int decimals = 2, string? culture = null)
        => _numbersHelper.FormatNumber(number, decimals, culture);
    public string FormatPercentage(double value, int decimals = 2)
        => _numbersHelper.FormatPercentage(value, decimals);
    public string AbbreviateNumber(long number)
        => _numbersHelper.AbbreviateNumber(number);

}

