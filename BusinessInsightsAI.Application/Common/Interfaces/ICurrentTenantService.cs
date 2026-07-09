namespace BusinessInsightsAI.Application.Common.Interfaces;

public interface ICurrentTenantService
{
    string? TenantId { get; }
}