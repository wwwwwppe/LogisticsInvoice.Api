using LogisticsInvoice.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogisticsInvoice.Api.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<BusinessParty> BusinessParties => Set<BusinessParty>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<BusinessParty>();

        entity.ToTable("BusinessParties");
        entity.HasKey(item => item.Id);
        entity.Property(item => item.Code).HasMaxLength(30).IsRequired();
        entity.HasIndex(item => item.Code).IsUnique();
        entity.Property(item => item.Name).HasMaxLength(100).IsRequired();
        entity.Property(item => item.Type).HasConversion<string>().HasMaxLength(20);
        entity.Property(item => item.ContactName).HasMaxLength(50);
        entity.Property(item => item.Phone).HasMaxLength(30);
        entity.Property(item => item.Address).HasMaxLength(200);
    }
}
