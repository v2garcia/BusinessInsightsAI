namespace  BusinessInsightsAI.Domain.Entities;

public class Tenant
{
    public string Id { get; set; } = null!;
    public string? Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}