using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.Entities;

namespace WebApplication1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Merchandise> Merchandises { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Relationships
        modelBuilder.Entity<Merchandise>()
            .HasOne(p => p.Collection)
            .WithMany(c => c.Merchandises)
            .HasForeignKey(p => p.CollectionId);

        modelBuilder.Entity<Purchase>()
            .HasOne(o => o.User)
            .WithMany(u => u.Purchases)
            .HasForeignKey(o => o.UserId);

        modelBuilder.Entity<PurchaseItem>()
            .HasOne(oi => oi.Purchase)
            .WithMany(o => o.PurchaseItems)
            .HasForeignKey(oi => oi.PurchaseId);
            
         modelBuilder.Entity<PurchaseItem>()
            .HasOne(oi => oi.Merchandise)
            .WithMany()
            .HasForeignKey(oi => oi.MerchandiseId);

        // Soft Delete Filter
        modelBuilder.Entity<Merchandise>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Collection>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Purchase>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<PurchaseItem>().HasQueryFilter(x => !x.IsDeleted);
    }
}
