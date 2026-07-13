namespace BusinessInsightsAI.Application.Common.Events;

public record InvoicesIngestedEvent
{
    public string TenantId { get; init; } = null!;
    public List<Guid> InvoiceIds { get; init; } = new();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
    
}
