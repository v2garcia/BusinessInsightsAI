using BusinessInsightsAI.Application.Common.Interfaces;
using BusinessInsightsAI.Infrastructure.Persistence;
using BusinessInsightsAI.Infrastructure.Services;
using BusinessInsightsAI.WebApi.Middlewares;
using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//agregando el middleware
app.UseMiddleware<TenantMiddleware>();

app.UseAuthorization();
app.MapControllers();


app.Run();
