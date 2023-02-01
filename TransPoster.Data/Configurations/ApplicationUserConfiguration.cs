﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data.Models;

namespace TransPoster.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(e => e.PasswordUpdatedAt).HasDefaultValueSql("getdate()");

        builder.HasMany(au => au.UserHistory)
            .WithOne(uh => uh.User);
    }
}