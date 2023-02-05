#nullable disable

namespace TransPoster.Mvc.DataTables.Model;

public sealed class AjaxDataRequest
{
    public int Draw { get; set; }
    public int Start { get; set; }
    public int Length { get; set; }
    public List<AjaxDataRequestOrder> Order { get; set; } = new List<AjaxDataRequestOrder>();
    public List<AjaxDataRequestColumn> Columns { get; set; } = new List<AjaxDataRequestColumn>();
    public AjaxDataRequestSearch Search { get; set; }
}