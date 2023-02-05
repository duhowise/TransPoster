using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using TransPoster.Mvc.DataTables.Attributes;

namespace TransPoster.Mvc.DataTables.Client;

public static class ExportSettingsHelper
{
    public static IEnumerable<ExportableColumnSetting> GetColumnSettings(Type type)
    {
        var settings = (from prop in type.GetTypeInfo().GetProperties()
                        let att = prop.GetCustomAttribute<ColumnSettingsAttribute>(true)
                        let areaAtt = prop.GetCustomAttribute<AreaPositionAttribute>(true)
                        let display = prop.GetCustomAttribute<DisplayAttribute>(true)
                        where att != null
                        orderby areaAtt?.Position, areaAtt?.PositionWithinArea, att.Position
                        select new ExportableColumnSetting
                        {
                            Name = prop.Name,
                            FilterItemsSource = att.FilterItemsSource,
                            FilterType = att.FilterType,
                            Orderable = att.Orderable,
                            Title = display.GetName(),
                            DateFormat = att.DateFormat,
                            FilterItemListType = att.FilterItemListType
                        }).ToList();

        return settings;
    }

    public static string ReplaceModelPlaceholders(string s, object model)
    {
        if (model == null)
        {
            return s;
        }

        var properties = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        const string pattern = "({model:)(\\w+)(})";
        var result = Regex.Replace(s, pattern, Replace);

        string Replace(Match match)
        {
            var propertyName = match.Groups[2].Value;
            var property = properties.First(p => string.Equals(p.Name, propertyName, StringComparison.InvariantCultureIgnoreCase));
            var val = property.GetValue(model).ToString();

            return val;
        }

        return result;
    }
}