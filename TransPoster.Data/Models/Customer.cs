using TransPoster.Data.Interfaces;

namespace TransPoster.Data.Models;

public class Customer :IIdName
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; }
}