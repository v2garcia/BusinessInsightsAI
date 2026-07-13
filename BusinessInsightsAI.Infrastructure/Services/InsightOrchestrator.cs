
using System.Data;
using Microsoft.Extensions.Logging;
using BusinessInsightsAI.Application.Common.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BusinessInsightsAI.Infrastructure.Services;

public class InsightOrchestrator : IInsightOrchestrator
{
    private readonly IAIService _aiService;
    private readonly ICurrentTenantService  _tenantService;
    private readonly string _connectionString;
    private readonly ILogger<InsightOrchestrator> _logger;

    public InsightOrchestrator(
        IAIService aiService,
        ICurrentTenantService tenantService,
        IConfiguration configuration,
        ILogger<InsightOrchestrator> logger)
    {
        _aiService = aiService;
        _tenantService = tenantService;
        _logger = logger;
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentNullException(nameof(configuration));

    }
    
    public async Task<object> AskInsightAsync(string userPromt)
    {
        string currentTenatId = _tenantService.TenantId
                                ?? throw new InvalidOperationException(
                                    "No se ha establecido un contexto de Tenant valido");
        _logger.LogInformation("Solicitando traduccion de lenguaje natural para el Tenant: {TenantId}",currentTenatId);
        
        string sqlQuery = await _aiService.ChatToSqlQueryAsync(userPromt, currentTenatId);
        
        _logger.LogInformation("SQL Generado de forma segura por IA:{Query}",sqlQuery);

        using IDbConnection db = new SqlConnection(_connectionString);
        var queryResult = await db.QueryAsync(sqlQuery, new { TenantId = currentTenatId });

        return new
        {
            Promt = userPromt,
            ExecuteSql = sqlQuery,
            Data = queryResult
        };
    }

}