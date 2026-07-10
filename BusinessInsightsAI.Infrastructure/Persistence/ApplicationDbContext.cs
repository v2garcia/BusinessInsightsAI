using Azure.Core.Pipeline;
using BusinessInsightsAI.Application.Common.Interfaces;
using BusinessInsightsAI.Domain.Common;
using BusinessInsightsAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessInsightsAI.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentTenantService _currentTenantService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentTenantService currentTenantService) : base(options)
    {
        _currentTenantService = currentTenantService;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Invoice>  Invoices => Set<Invoice>();
    public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
        });
        modelBuilder.Entity<Product>(entity => entity.Property(p => p.Price).HasPrecision(18, 2));
        modelBuilder.Entity<Product>(entity => entity.Property(p => p.Cost).HasPrecision(18, 2));
        modelBuilder.Entity<Invoice>(entity => entity.Property(i => i.TotalAmount).HasPrecision(18, 2));
        modelBuilder.Entity<Invoice>(entity => entity.Property(i => i.TotalTax).HasPrecision(18, 2));
        modelBuilder.Entity<InvoiceLine>(entity => entity.Property(l => l.UnitPrice).HasPrecision(18, 2));
        modelBuilder.Entity<InvoiceLine>(entity => entity.Property(l => l.Discount).HasPrecision(18, 2));
        

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IMustHaveTenant).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType).
                    HasQueryFilter(ConvertFilterExpression(entityType.ClrType));
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.TenantId = _currentTenantService.TenantId
                                            ?? throw new InvalidOperationException("No se puede insertar datos sin un Tenant valido en el contexto");
                    break;
                
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private System.Linq.Expressions.LambdaExpression ConvertFilterExpression(Type entityType)
    {
        var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "e");
        var tenantIdProperty = System.Linq.Expressions.Expression.Property(parameter, nameof(IMustHaveTenant.TenantId));
        var currentTenantId = System.Linq.Expressions.Expression.Constant(_currentTenantService.TenantId);
        var equality = System.Linq.Expressions.Expression.Equal(tenantIdProperty, currentTenantId);
        return System.Linq.Expressions.Expression.Lambda(equality, parameter);
    }
    
    
    
}

