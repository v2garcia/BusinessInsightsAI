using BusinessInsightsAI.Application.Common.Interfaces;


namespace  BusinessInsightsAI.Infrastructure.Services;

public class CurrentTenantService : ICurrentTenantService
{
    public string? TenantId { get; private set; }

    public void SetTenantId(string tenantId)
    {
        if (!string.IsNullOrEmpty(tenantId))
        {
            throw new InvalidOperationException("El Tenant de la solicitud ya ha sido establecido.");
        }
        
        TenantId = tenantId;
    }
}

