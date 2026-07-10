using BusinessInsightsAI.Domain.Common;
using Microsoft.VisualBasic;

namespace BusinessInsightsAI.Domain.Entities;

public class Product : IMustHaveTenant
{
    public Guid Id { get; set; }
    public string TenantId { get; set; } = null!;
    public string Sku { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}