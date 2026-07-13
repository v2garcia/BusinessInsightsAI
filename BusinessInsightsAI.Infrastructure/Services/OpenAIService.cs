using System.ClientModel;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using BusinessInsightsAI.Application.Common.Interfaces;

namespace BusinessInsightsAI.Infrastructure.Services;

public class OpenAIService : IAIService
{
    private readonly ChatClient _chatClient;

    public OpenAIService(IConfiguration configuration)
    {
        var apikey = configuration["OpenAI:ApiKey"]
                ?? throw new ArgumentNullException("La Api key de OpenAI no esta Configurada.");

        _chatClient = new ChatClient(model: "gpt-4o-mini", apiKey: apikey);
        
    }

    public async Task<string> ChatToSqlQueryAsync(string userPrompt, string tenantId)
    {
        string dbSchemaContext  = @"
            Tablas disponibles en el sistema:
            - Customers (Id uniqueidentifier, Name varchar, TaxId varchar)
            - Products (Id uniqueidentifier, Sku varchar, Name varchar, Price decimal, Cost decimal)
            - Invoices (Id uniqueidentifier, InvoiceNumber varchar, CustomerId uniqueidentifier, TotalAmount decimal, TotalTax decimal, IssuedAt datetime, TenantId varchar)
            - InvoiceLines (Id uniqueidentifier, InvoiceId uniqueidentifier, ProductId uniqueidentifier, Quantity int, UnitPrice decimal)
        ";
        string systemInstruction = $@"
            Eres un asistente experto en bases de datos SQL Server transaccionales.
            Tu única tarea es traducir la pregunta en lenguaje natural del usuario a una consulta SQL válida de lectura (SELECT).
            
            REGLAS CRÍTICAS DE SEGURIDAD:
            1. Solo puedes generar consultas SELECT. Está estrictamente prohibido generar consultas de modificación (INSERT, UPDATE, DELETE, DROP, ALTER).
            2. OBLIGATORIO: Toda consulta a la tabla 'Invoices' DEBE incluir obligatoriamente la cláusula de filtrado por inquilino: 'TenantId = @TenantId'.
            3. Devuelve ÚNICAMENTE el código SQL limpio, sin bloques de formato markdown (sin ```sql), sin explicaciones y en una sola línea.
            
            Esquema de la base de datos:
            {dbSchemaContext}
        ";

        List<ChatMessage> messages = new()
        {
            new SystemChatMessage(systemInstruction),
            new UserChatMessage(
                $"Pregunta del Usuario: '{userPrompt}'. genera el SQL restrictivo para el TenantId: '{tenantId}'  ")
        };

        ChatCompletion completion = await _chatClient.CompleteChatAsync(messages);

        string generateSql = completion.Content[0].Text.Trim();
        if (generateSql.Contains("DROP", StringComparison.OrdinalIgnoreCase) ||
            generateSql.Contains("DELETE", StringComparison.OrdinalIgnoreCase) ||
            generateSql.Contains("UPDATE", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Alerta de seguridad: La IA intento generar una consutla maliciosa o no autorizada.");
        }
        
        return generateSql;

    }
}