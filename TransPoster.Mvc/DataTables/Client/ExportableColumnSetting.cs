namespace TransPoster.Mvc.DataTables.Client;

public sealed class ExportableColumnSetting
{
    public string Name { get; set; }
    public string Title { get; set; }
    public string FilterItemsSource { get; set; }
    public FilterType FilterType { get; set; }
    public bool Orderable { get; set; }
    public string DateFormat { get; set; }
    public Type FilterItemListType { get; set; }
}