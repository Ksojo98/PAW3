using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAW3.Architecture.Helpers
{
        public interface IDatesHelper
        {
            string FormatDate(DateTime date, string format = "yyyy-MM-dd", CultureInfo? culture = null);
            string FormatDateLong(DateTime date, CultureInfo? culture = null);
            string TimeAgo(DateTime date);
            int CalculateAge(DateTime birthDate);
            string ToRelativeDate(DateTime date);
        }
        public class DatesHelper : IDatesHelper
        {

            public readonly CultureInfo _defaultCulture;

        public DatesHelper()
        {
            _defaultCulture = CultureInfo.CurrentCulture;
        }
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
    }
}
