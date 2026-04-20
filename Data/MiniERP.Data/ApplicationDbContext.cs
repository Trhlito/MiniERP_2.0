using Microsoft.EntityFrameworkCore;
using MiniERP.Data.Entities;

namespace MiniERP.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Stock> Stock { get; set; }
    public DbSet<StockMovement> StockMovements { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.CustomerType)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.CompanyName).HasMaxLength(200);
            entity.Property(x => x.FirstName).HasMaxLength(100);
            entity.Property(x => x.LastName).HasMaxLength(100);
            entity.Property(x => x.Email).HasMaxLength(150);

            entity.HasCheckConstraint(
                "CK_Customers_CustomerType",
                "CustomerType IN ('Person', 'Company')");
        });
    }
}