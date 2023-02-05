using TransPoster.Data.Interfaces;

namespace TransPoster.Data.Models;

public class Product : IIdName
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
}