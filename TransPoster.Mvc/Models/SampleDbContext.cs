using Microsoft.EntityFrameworkCore;

namespace TransPoster.Mvc.Models;

public sealed class SampleDbContext : DbContext
{
    public SampleDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category() { Id = 1, Name = "Computers" },
            new Category() { Id = 2, Name = "Screens" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product()
            {
                Id = 1,
                Name = "HP Laptop",
                CategoryId = 1,
                CreateDate = DateTime.Today.AddDays(-1),
                IsActive = true,
            },
            new Product()
            {
                Id = 2,
                Name = "Dell Desktop",
                CategoryId = 1,
                CreateDate = DateTime.Today,
                IsActive = true,
            },
            new Product()
            {
                Id = 3,
                Name = "Workstation",
                CreateDate = DateTime.Today,
                CategoryId = 1
            },
            new Product()
            {
                Id = 4,
                Name = "Large",
                CategoryId = 2,
                CreateDate = DateTime.Today,
                IsActive = true
            },
            new Product()
            {
                Id = 5,
                Name = "Small",
                CreateDate = DateTime.Today,
                CategoryId = 2
            }
        );
    }
}
