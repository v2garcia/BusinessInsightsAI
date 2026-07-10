
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;  
using BusinessInsightsAI.Application.Common.Interfaces;

namespace BusinessInsightsAI.Infrastructure.Services;

public class SalesAnalyticsService : ISalesAnalyticsService
{
    private readonly string _connectionString;

    public SalesAnalyticsService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException(nameof(configuration));
        
    }

    public async Task<decimal> GetTotalSalesAsync(string tenantId, DateTime start, DateTime end)
    {

        const string sql = @" 
            SELECT COALESCE(SUM(TotalAmount),0) 
            FROM Invoices
            WHERE TenantId = @TenantId AND CreatedAt BETWEEN @Start AND @End";
        
        using IDbConnection db = new SqlConnection(_connectionString);
        return await db.ExecuteScalarAsync<decimal>(sql, new { TenantId = tenantId, Start = start, End = end });
        
    }
}