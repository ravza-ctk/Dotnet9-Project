using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Core.Entities;

namespace WebApplication1.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // 1. Seed Collections
        if (!context.Collections.Any())
        {
            await context.Collections.AddRangeAsync(
                new Collection { Name = "Elektronik" },
                new Collection { Name = "Kitap" }
            );
            await context.SaveChangesAsync();
        }

        // 2. Seed Merchandises
        if (!context.Merchandises.Any())
        {
            var electronics = context.Collections.FirstOrDefault(c => c.Name == "Elektronik");
            var books = context.Collections.FirstOrDefault(c => c.Name == "Kitap");

            if (electronics != null && books != null)
            {
                await context.Merchandises.AddRangeAsync(
                    new Merchandise { Name = "Laptop", Price = 25000, Stock = 10, CollectionId = electronics.Id },
                    new Merchandise { Name = "Telefon", Price = 15000, Stock = 20, CollectionId = electronics.Id },
                    new Merchandise { Name = "Roman", Price = 150, Stock = 100, CollectionId = books.Id }
                );
                await context.SaveChangesAsync();
            }
        }

        // 3. Seed User
        if (!context.Users.Any())
        {
            var seedUser = new User
            {
                Username = "seed_user",
                Email = "seed@demo.com",
                Role = "Admin",
                PasswordHash = "12345" // Plain text for demo
            };
            await context.Users.AddAsync(seedUser);
            await context.SaveChangesAsync();
        }

        // 4. Seed Purchases
        if (!context.Purchases.Any())
        {
            var user = context.Users.FirstOrDefault(u => u.Username == "seed_user");
            var laptop = context.Merchandises.FirstOrDefault(p => p.Name == "Laptop");

            if (user != null && laptop != null)
            {
                var purchase = new Purchase
                {
                    UserId = user.Id,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = laptop.Price * 1, // 1 Laptop
                    PurchaseItems = new List<PurchaseItem>
                    {
                        new PurchaseItem
                        {
                            MerchandiseId = laptop.Id,
                            Quantity = 1,
                            UnitPrice = laptop.Price
                        }
                    }
                };

                await context.Purchases.AddAsync(purchase);
                await context.SaveChangesAsync();
            }
        }
    }
}
