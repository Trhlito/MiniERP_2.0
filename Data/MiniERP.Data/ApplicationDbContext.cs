using Microsoft.EntityFrameworkCore;
using MiniERP.Data.Entities;

namespace MiniERP.Data;

// Databázový kontext aplikace
public class ApplicationDbContext : DbContext
{
    // Konstruktor pro databázový kontext
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSet pro zákazníky
    public DbSet<Customer> Customers => Set<Customer>();

    // DbSet pro produkty
    public DbSet<Product> Products { get; set; }

    // DbSet pro objednávky
    public DbSet<Order> Orders { get; set; }

    // DbSet pro položky objednávek
    public DbSet<OrderItem> OrderItems { get; set; }

    // DbSet pro sklad
    public DbSet<Stock> Stock { get; set; }

    // DbSet pro auditní pohyby skladu
    public DbSet<StockMovement> StockMovements { get; set; }

    // DbSet pro faktury
    public DbSet<Invoice> Invoices { get; set; }

    // DbSet pro položky faktur
    public DbSet<InvoiceItem> InvoiceItems { get; set; }

    // DbSet pro platby
    public DbSet<Payment> Payments { get; set; }

    // Konfigurace modelu databáze
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //  Konfigurace entity Customer 
        modelBuilder.Entity<Customer>(entity =>
        {
            //  Mapování na tabulku Customers a check constraint
            entity.ToTable("Customers", t =>
            {
                //  Omezení povolených hodnot pro CustomerType
                t.HasCheckConstraint(
                    "CK_Customers_CustomerType",
                    "CustomerType IN ('Person', 'Company')");
            });

            //  Nastavení primárního klíče
            entity.HasKey(x => x.Id);

            //  Konfigurace vlastnosti CustomerType
            entity.Property(x => x.CustomerType)
                .HasMaxLength(20)
                .IsRequired();

            //  Konfigurace délky textových polí
            entity.Property(x => x.CompanyName).HasMaxLength(200);
            entity.Property(x => x.FirstName).HasMaxLength(100);
            entity.Property(x => x.LastName).HasMaxLength(100);
            entity.Property(x => x.Email).HasMaxLength(150);
        });
    }
}