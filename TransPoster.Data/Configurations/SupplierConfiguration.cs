using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransPoster.Data.Models;

namespace TransPoster.Data.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        Seed(builder);
    }
    private static void Seed(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasData(
            new Supplier
            {
                Id = 1,
                Name = "Gridlock Inc.",
                SupplierNo = "UD234567",
                Location = "Tel Aviv",
                CreatedAt = DateTime.UtcNow,
            },
            new Supplier
            {
                Id = 2,
                Name = "Big Homes ",
                SupplierNo = "UD2111117",
                Location = "Tel Aviv",
                CreatedAt = DateTime.UtcNow,
            }
        );
    }
}