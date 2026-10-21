namespace NorthWind.Sales.Entities.Dtos.Invoices;

public class OrderInvoiceDetailDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public decimal Subtotal => Math.Round(UnitPrice * Quantity, 2);
}
