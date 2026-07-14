
using Xunit;
using FluentAssertions;
using BusinessInsightsAI.Domain.Entities;

namespace BusinessInsightsAI.UnitTests;

public class TenantIsolationTests
{
    [Fact]
    public void Customer_Should_Implement_MultiTenant_Interface_Strictly()
    {
        var customer = new Customer { Name = "Empresa Dominicana S.A", TaxId = "123456789" };
        customer.Should().BeAssignableTo<BusinessInsightsAI.Domain.Common.IMustHaveTenant>();
        
    }
}