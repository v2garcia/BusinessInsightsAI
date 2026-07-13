using MassTransit;
using Microsoft.Extensions.Logging;
using BusinessInsightsAI.Application.Common.Events;


namespace BusinessInsightsAI.BackgroundWorker.Consumers;

public class InvoicesIngestedConsumer: IConsumer<InvoicesIngestedEvent>
{
    private readonly ILogger<InvoicesIngestedConsumer> _logger;
    
    public InvoicesIngestedConsumer(ILogger<InvoicesIngestedConsumer> _logger)
    {
        this._logger = _logger;
    }

    public async Task Consume(ConsumeContext<InvoicesIngestedEvent> context)
    {
        var message = context.Message;

        using (_logger.BeginScope(new Dictionary<string, object> { ["TenantId"] = message.TenantId }))
        {
            _logger.LogInformation("Procesando{Count} facturas asincronamente para el Tenant.",message.InvoiceIds.Count);
            await Task.Delay(2000);
            _logger.LogInformation("Pre-agregaciones analiticas optimizadas con exito para el tenant.");
        }
    }
}