using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data.Models;

namespace TransPoster.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        Seed(builder);
    }

    private static void Seed(EntityTypeBuilder<Customer> builder)
    {
        builder.HasData(
            new Customer
            {
                Id = 1,
                Name = "Wise Duho",
                Address = "New Port",
                CreatedAt = DateTime.UtcNow,
            },
            new Customer
            {
                Id = 2,
                Name = "Komla",
                Address = "Trasaco",
                CreatedAt = DateTime.UtcNow,
            }
        );
    }
}