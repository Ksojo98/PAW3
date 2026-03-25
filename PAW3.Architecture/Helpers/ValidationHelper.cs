using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PAW3.Architecture.Helpers
{
    public interface IValidationHelper
    {
        bool TryParseDate(string input, out DateTime result);
        bool IsValidEmail(string email);
    }
    public class ValidationHelper : IValidationHelper
    {
        public bool TryParseDate(string input, out DateTime result)
        {
            return DateTime.TryParse(input, out result);
        }

        public bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
