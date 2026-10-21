using System;

namespace NorthWind.Sales.Entities.Dtos.Invoices;

public class OrderSummaryDto
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string ShipCountry { get; set; } = string.Empty;
    public string ShipCity { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public int ItemCount { get; set; }
    public decimal Total { get; set; }
}
