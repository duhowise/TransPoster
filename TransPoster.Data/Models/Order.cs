using TransPoster.Data.Interfaces;

namespace TransPoster.Data.Models;

public class Order : IIdName
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string OrderNo { get; set; }
    public decimal Total { get; set; }
    public bool Delivered { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public Customer Customer { get; set; }
}