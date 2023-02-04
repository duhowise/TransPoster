namespace TransPoster.Data.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
}