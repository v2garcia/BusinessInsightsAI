
namespace BusinessInsightsAI.Application.Common.Interfaces;

public interface IInsightOrchestrator
{
    Task<object> AskInsightAsync(string userPromt);
}