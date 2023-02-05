using System.Text;
using EnumsNET;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TransPoster.Mvc.DataTables;
using TransPoster.Mvc.DataTables.Client;

namespace TransPoster.Mvc.Mvc.TagHelpers;

[HtmlTargetElement("table", Attributes = "model-type")]
public sealed class DataTableTagHelper : TagHelper
{
    public Type ModelType { get; set; } = null!;
    public bool AddFirstColumn { get; set; }
    public bool AddLastColumn { get; set; }
    public object Model { get; set; } = null!;
    public string[] ExcludeColumns { get; set; } = null!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;

        var builder = new StringBuilder();
        builder.AppendLine("<thead>");
        builder.AppendLine("<tr data-dt-title>");

        if (AddFirstColumn)
        {
            builder.Append("<th></th>");
        }

        var columnSettings = ExportSettingsHelper.GetColumnSettings(ModelType);

        if (ExcludeColumns is { })
        {
            columnSettings = columnSettings.Where(c => !ExcludeColumns.Contains(c.Name));
        }

        foreach (var clmn in columnSettings)
        {
            builder.AppendLine("<th");

            void AppendColumnData(string name, string value)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    builder.Append($" data-column-{name}='{value}'");
                }
            }

            AppendColumnData("name", clmn.Name);

            if (!clmn.Orderable)
            {
                AppendColumnData("orderable", "false");
            }

            AppendColumnData("dateformat", clmn.DateFormat);

            builder.Append($">{clmn.Title}</th>");
        }

        if (AddLastColumn)
        {
            builder.AppendLine("<th></th>");
        }

        builder.AppendLine("</tr>");

        // filter row
        builder.AppendLine("<tr data-dt-filter>");

        if (AddFirstColumn)
        {
            builder.AppendLine("<th></th>");
        }

        foreach (var clmn in columnSettings)
        {
            string filter;
            switch (clmn.FilterType)
            {
                case FilterType.Text:
                    filter = "<th class='filter-text'></th>";
                    break;
                case FilterType.List:
                    if (clmn.FilterItemListType is { })
                    {
                        var options = string.Join('\r', Enums.GetMembers(clmn.FilterItemListType)
                            .Select(m => $"<option value='{m.GetUnderlyingValue()}'>{m.AsString(EnumFormat.DisplayName)}</option>"));
                        filter = $"<th class='filter-select'><select data-filter><option value=''></option>{options}</select></th>";
                    }
                    else
                    {
                        var itemsSource = ExportSettingsHelper.ReplaceModelPlaceholders(clmn.FilterItemsSource, Model);
                        filter = $"<th class='filter-select' data-filter-source='{itemsSource}'></th>";
                    }
                    break;
                case FilterType.DateRange:
                    filter = "<th class='filter-daterange'></th>";
                    break;
                case FilterType.Boolean:
                    filter = "<th class='filter-bool'></th>";
                    break;
                case FilterType.None:
                default:
                    filter = "<th></th>";
                    break;
            }

            builder.Append(filter);
        }

        if (AddLastColumn)
        {
            builder.Append("<th></th>");
        }

        builder.Append("</thead>");
        builder.Append("<tbody></tbody>");

        output.Content.SetHtmlContent(builder.ToString());
    }
}
