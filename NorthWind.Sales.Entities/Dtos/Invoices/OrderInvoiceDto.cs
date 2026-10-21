using System;
using System.Collections.Generic;
using System.Linq;

namespace NorthWind.Sales.Entities.Dtos.Invoices;

public class OrderInvoiceDto
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string ShipAddress { get; set; } = string.Empty;
    public string ShipCity { get; set; } = string.Empty;
    public string ShipCountry { get; set; } = string.Empty;
    public string ShipPostalCode { get; set; } = string.Empty;
    public List<OrderInvoiceDetailDto> Details { get; set; } = [];

    public decimal Subtotal => Math.Round(Details.Sum(d => d.Subtotal), 2);
    public decimal TaxRate => 0.15m;
    public decimal Tax => Math.Round(Subtotal * TaxRate, 2);
    public decimal Total => Math.Round(Subtotal + Tax, 2);
}
