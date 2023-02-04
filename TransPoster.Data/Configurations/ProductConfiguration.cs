using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data.Models;

namespace TransPoster.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        Seed(builder);
    }

    private static void Seed(EntityTypeBuilder<Product> builder)
    {
        builder.HasData(
            new Product
            {
                Id = 1,
                Name = "JBL Headset",
                Stock = 20,
                UnitPrice = (decimal)399.98,
                CreatedAt = DateTime.UtcNow,
            },
            new Product
            {
                Id = 2,
                Name = "Macbook Pro",
                Stock = 50,
                UnitPrice = (decimal)1988.98,
                CreatedAt = DateTime.UtcNow,
            }
        );
    }
}