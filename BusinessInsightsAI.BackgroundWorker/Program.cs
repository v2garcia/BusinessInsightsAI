using BusinessInsightsAI.BackgroundWorker;
using BusinessInsightsAI.BackgroundWorker.Consumers;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(x =>
{
        x.AddConsumer<InvoicesIngestedConsumer>();
        x.UsingRabbitMq(((context, cfg) =>
        {
                cfg.Host("localhost", "/", h =>
                {
                        h.Username("guest");
                        h.Password("guest");
                });
        }));
});

var host = builder.Build();
host.Run();
