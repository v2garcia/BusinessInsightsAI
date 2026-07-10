namespace BusinessInsightsAI.Application.Common.Interfaces;

public interface ISalesAnalyticsService
{
    Task<decimal> GetTotalSalesAsync(string tenantId,DateTime start , DateTime end);
}