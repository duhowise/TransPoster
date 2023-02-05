namespace TransPoster.Mvc.DataTables.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ColumnSettingsAttribute : Attribute
{
    public bool Orderable { get; set; } = true;
    public FilterType FilterType { get; set; }
    public Type FilterItemListType { get; set; }
    public string FilterItemsSource { get; set; }
    public string DateFormat { get; set; }
    public int Position { get; set; }
}