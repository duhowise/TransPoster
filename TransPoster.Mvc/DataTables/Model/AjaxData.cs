using System.Collections;
using System.Text.Json.Serialization;

namespace TransPoster.Mvc.DataTables.Model;

#nullable disable

// JsonProperty: Because dataTables expect camelCase, but column names should be PascalCase.
public sealed class AjaxData
{
    [JsonPropertyName("draw")]
    public int? Draw { get; set; }

    [JsonPropertyName("recordsTotal")]
    public int? RecordsTotal { get; set; }

    [JsonPropertyName("recordsFiltered")]
    public int? RecordsFiltered { get; set; }

    [JsonPropertyName("data")]
    public IEnumerable Data { get; set; }

    [JsonPropertyName("error")]
    public string Error { get; set; }

    public static AjaxData CreateSimple(AjaxDataRequest request, ICollection data) => new()
    {
        Draw = request.Draw,
        RecordsTotal = data.Count,
        RecordsFiltered = data.Count,
        Data = data
    };

    public static AjaxData Create(int draw, int totalCount, int filteredCount, ICollection data) => new()
    {
        Draw = draw,
        RecordsTotal = totalCount,
        RecordsFiltered = filteredCount,
        Data = data
    };

    public static AjaxData Empty(AjaxDataRequest request) => new()
    {
        Draw = request.Draw,
        RecordsTotal = 0,
        RecordsFiltered = 0,
        Data = Array.Empty<object>()
    };

    public static AjaxData Empty(AjaxDataRequest request, int totalCount) => new()
    {
        Draw = request.Draw,
        RecordsTotal = totalCount,
        RecordsFiltered = 0,
        Data = Array.Empty<object>()
    };
}