using BusinessInsightsAI.Domain.Common;

namespace BusinessInsightsAI.Domain.Entities;

public class Invoice : IMustHaveTenant
{
    public Guid Id { get; set; }
    public string TenantId { get; set; } = null!;
    public string InvoiceNumber { get; set; } = null!;
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public decimal TotalTax { get; set; }
    public DateTime IssuedAt { get; set; }

    public List<InvoiceLine> Lines { get; set; } = new();

}

public class InvoiceLine
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
}