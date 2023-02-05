using System.ComponentModel.DataAnnotations;
using TransPoster.Mvc.DataTables;
using TransPoster.Mvc.DataTables.Attributes;
using TransPoster.Mvc.Models;

namespace TransPoster.Mvc.Data.Products;

public sealed class ProductViewModel
{
    [Display(Name = "#")]
    [ColumnSettings(FilterType = FilterType.Text)]
    [SourceField]
    public int Id { get; set; }

    [Display(Name = "Name")]
    [ColumnSettings(FilterType = FilterType.Text)]
    [SourceField]
    public string Name { get; set; } = null!;

    [Display(Name = "Unit Price")]
    [ColumnSettings(FilterType = FilterType.Decimal)]
    [SourceField]
    public decimal UnitPrice { get; set; }

    [Display(Name = "Stock")]
    [ColumnSettings(FilterType = FilterType.Int)]
    [SourceField]
    public int Stock { get; set; }

    //[Display(Name = "Category")]
    //[ColumnSettings(FilterType = FilterType.List, FilterItemsSource = "Filters/Categories")]
    //[NavigationSource(nameof(Product.Category))]
    //public string CategoryName { get; set; } = null!;

    [Display(Name = "Create Date")]
    [ColumnSettings(FilterType = FilterType.DateRange, DateFormat = KnownFormats.ShortDateFormat)]
    [SourceField]
    public DateTime CreatedAt { get; set; }

    //[Display(Name = "Is Active")]
    //[ColumnSettings(FilterType = FilterType.Boolean)]
    //[SourceField]
    //public bool IsActive { get; set; }
}
