using System.Globalization;

namespace TransPoster.Mvc.DataTables.Helpers;

public static class DateTimeHelpers
{
    public static DateTime? TryParse(string s, string format)
    {
        var culture = CultureInfo.CurrentCulture;

        if (DateTime.TryParseExact(s, format, culture, DateTimeStyles.None, out DateTime d))
        {
            return d;
        }

        return null;
    }
}