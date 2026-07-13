using BusinessInsightsAI.Application.Common.Interfaces;
using BusinessInsightsAI.Infrastructure.Persistence;
using BusinessInsightsAI.Infrastructure.Services;
using BusinessInsightsAI.WebApi.Middlewares;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//agregando los servicios Multi-tenant
builder.Services.AddScoped<CurrentTenantService>();
builder.Services.AddScoped<ICurrentTenantService>(provider => provider.GetRequiredService<CurrentTenantService>());

//configuracion  de servicio de sql
builder.Services.AddDbContext<ApplicationDbContext>((ServiceProvider, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddScoped<ISalesAnalyticsService, SalesAnalyticsService>();
builder.Services.AddScoped<IAIService, OpenAIService>();
builder.Services.AddScoped<IInsightOrchestrator, InsightOrchestrator>();


//SeriLog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "BusinessInsightsAI.WebApi")
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//agregando el middleware
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<TenantMiddleware>();

app.UseAuthorization();
app.MapControllers();


app.Run();
