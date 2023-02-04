using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data.Models;

namespace TransPoster.Data.Configurations;

public class UserHistoryConfiguration : IEntityTypeConfiguration<UserHistory>
{
    public void Configure(EntityTypeBuilder<UserHistory> builder)
    {
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("getdate()");

        builder.HasOne(uh => uh.User)
            .WithMany(au => au.UserHistory);
    }
}