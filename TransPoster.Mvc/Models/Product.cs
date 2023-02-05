using TransPoster.Data.Interfaces;

namespace TransPoster.Mvc.Models;

public class Product : IIdName
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsActive { get; set; }

    public Category Category { get; set; }
}
