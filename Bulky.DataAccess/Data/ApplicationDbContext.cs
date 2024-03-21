

using Bulky.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bulky.DataAcess.Data
{
    public class ApplicationDbContext: IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)  
        {
                
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DispalyOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DispalyOrder = 2 },
                new Category { Id = 3, Name = "History", DispalyOrder = 3 }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product 
                { 
                    Id = 1,
                    Title = "Cotton Candy",
                    Author = "Cindy Smith",
                    Description = "Cindy loves cotton candy",
                    ISBN = "CC1111111111",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = 1,
                    ImagUrl = ""
                },
                new Product
                {
                    Id = 2,
                    Title = "Rock in the Ocean",
                    Author = "Rayan Gucci",
                    Description = "Rock in the ocean",
                    ISBN = "RG222222222",
                    ListPrice = 80,
                    Price = 75,
                    Price50 = 70,
                    Price100 = 65,
                    CategoryId = 2,
                    ImagUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae soldales libra",
                    ISBN = "NH333333333",
                    ListPrice = 100,
                    Price = 85,
                    Price50 = 80,
                    Price100 = 75,
                    CategoryId = 1,
                    ImagUrl = ""
                }
                );
        }
    }
}
