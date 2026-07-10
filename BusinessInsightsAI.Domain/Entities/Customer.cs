using BusinessInsightsAI.Domain.Common;

namespace BusinessInsightsAI.Domain.Entities;

public class Customer : IMustHaveTenant
{
    public Guid Id { get; set; }
    public string TenantId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string TaxId { get; set; } = null!;
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
}

