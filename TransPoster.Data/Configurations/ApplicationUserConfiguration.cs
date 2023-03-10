using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data.Models;

namespace TransPoster.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(e => e.PasswordUpdatedAt).HasDefaultValueSql("getdate()");
        builder.Property(e => e.IsActive).HasDefaultValue(true);


        builder.HasMany(au => au.UserHistory)
            .WithOne(uh => uh.User);
    }
}