

using BusinessInsightsAI.Application.Common.Events;
using BusinessInsightsAI.Domain.Entities;
using BusinessInsightsAI.Infrastructure.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Responses;

namespace BusinessInsightsAI.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    
    public InvoiceController (ApplicationDbContext context , IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
        
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> Bulk([FromBody] List<InvoiceDto> invoicesDto,
        [FromHeader(Name = "X-Tenant-Id")] string tenantId)
    {
        if (string.IsNullOrEmpty(tenantId)) return BadRequest("X-Tenant-Id header es requerido");
        
        var invoiceIds = new List<Guid>();
        foreach (var dto in invoicesDto)
        {
           var invoice = new Invoice
           {
               Id = Guid.NewGuid(),
               InvoiceNumber = dto.InvoiceNumber,
               CustomerId = dto.CustomerId,
               TotalAmount = dto.TotalAmount,
               TotalTax =  dto.TotalTax,
               IssuedAt = dto.IssuedAt,
               Lines = dto.Lines.Select(l => new InvoiceLine
               {
                   Id = Guid.NewGuid(),
                   ProductId  = l.ProductId,
                   Quantity = l.Quantity,
                   UnitPrice = l.UnitPrice
               
               }).ToList()
           };
           _context.Invoices.Add(invoice);
           invoiceIds.Add(invoice.Id);
           
        }
        
        await _context.SaveChangesAsync();
        
        await _publishEndpoint.Publish( new InvoicesIngestedEvent
        {
            TenantId = tenantId,
            InvoiceIds = invoiceIds
        });

        return Accepted(
            new { Message = "Lote de facturas aceptado y en proceso de analisis", Count = invoiceIds.Count });

    }
    
}

public record InvoiceDto(
    string InvoiceNumber,
    Guid CustomerId,
    decimal TotalAmount,
    decimal TotalTax,
    DateTime IssuedAt,
    List<InvoiceLineDto> Lines);
public record InvoiceLineDto (Guid ProductId, int Quantity, decimal UnitPrice);