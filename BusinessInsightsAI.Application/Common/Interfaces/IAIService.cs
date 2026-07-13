
namespace BusinessInsightsAI.Application.Common.Interfaces;


public interface IAIService
{
    Task<string> ChatToSqlQueryAsync(string userPrompt, string tenantId);
}