using System.IdentityModel.Tokens.Jwt;
using BusinessInsightsAI.Infrastructure.Services;

namespace BusinessInsightsAI.WebApi.Middlewares;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, CurrentTenantService tenantService)
    {
        string? tenantId = null;
        
        var autHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (autHeader != null && autHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = autHeader.Substring("Bearer ".Length).Trim();
            var handler  = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                tenantId = jwtToken.Claims.FirstOrDefault(c => c.Type == "tenant_id")?.Value;
            }
                
        }
        
        if (string.IsNullOrEmpty(tenantId))
        {
            tenantId = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();
        }

        if (!string.IsNullOrEmpty(tenantId))
        {
            tenantService.SetTenantId(tenantId);
        }
        await _next(context);
        
    }
}

