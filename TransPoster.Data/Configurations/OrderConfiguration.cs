using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data.Models;

namespace TransPoster.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        Seed(builder);
    }

    private static void Seed(EntityTypeBuilder<Order> builder)
    {
        builder.HasData(
            new Order
            {
                Id = 1,
                OrderNo = "OR12344",
                Delivered = true,
                DeliveredAt = DateTime.Now.AddDays(10),
                CreatedAt = DateTime.UtcNow,
            },
            new Order
            {
                Id = 2,
                OrderNo = "OR453324",
                Delivered = true,
                DeliveredAt = DateTime.Now.AddDays(15),
                CreatedAt = DateTime.UtcNow,
            }
        );
    }
}