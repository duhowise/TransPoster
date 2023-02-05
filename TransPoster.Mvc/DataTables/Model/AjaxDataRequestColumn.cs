#nullable disable

namespace TransPoster.Mvc.DataTables.Model;

public sealed class AjaxDataRequestColumn
{
    public string Name { get; set; }
    public string Data { get; set; }
    public bool Searchable { get; set; }
    public bool Orderable { get; set; }
    public AjaxDataRequestSearch Search { get; set; }
}