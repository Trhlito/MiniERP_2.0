using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniERP.Data.Entities;
using MiniERP.Data.Entities.Auth;

namespace MiniERP.Data;

// Databázový kontext propojuje ERP entity s ASP.NET Identity
public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // ERP moduly
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Stock> Stock { get; set; }
    public DbSet<StockMovement> StockMovements { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<Payment> Payments { get; set; }

    // Auth a bezpečnostní audit
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<AuthAuditLog> AuthAuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapování Identity tabulek na názvy v databázi
        modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
        modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");

        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<int>>()
            .ToTable("AspNetUserRoles");

        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<int>>()
            .ToTable("AspNetUserClaims");

        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<int>>()
            .ToTable("AspNetUserLogins");

        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<int>>()
            .ToTable("AspNetUserTokens");

        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>>()
            .ToTable("AspNetRoleClaims");

        // Audit autentizačních událostí
        modelBuilder.Entity<AuthAuditLog>(entity =>
        {
            entity.ToTable("AuthAuditLogs");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.UserName)
                .HasMaxLength(256);

            entity.Property(x => x.ActionType)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.IpAddress)
                .HasMaxLength(100);

            entity.Property(x => x.FailureReason)
                .HasMaxLength(255);

            entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(x => x.UserId);
            entity.HasIndex(x => x.ActionType);
            entity.HasIndex(x => x.CreatedAt);
        });

        // Základní konfigurace zákazníků
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers", t =>
            {
                // Typ zákazníka je omezený na fyzickou osobu nebo firmu
                t.HasCheckConstraint(
                    "CK_Customers_CustomerType",
                    "CustomerType IN ('Person', 'Company')");
            });

            entity.HasKey(x => x.Id);

            entity.Property(x => x.CustomerType)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.CompanyName).HasMaxLength(200);
            entity.Property(x => x.FirstName).HasMaxLength(100);
            entity.Property(x => x.LastName).HasMaxLength(100);
            entity.Property(x => x.Email).HasMaxLength(150);
        });
    }
}