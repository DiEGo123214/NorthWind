using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;

public class CreateOrderDetailViewModel
{
    private short _quantity = 1;

    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public short Quantity
    {
        get => _quantity;
        set => _quantity = value < 1 ? (short)1 : value;
    }
    public short UnitsInStock { get; set; }
    public int AvailableStock => Math.Max(0, UnitsInStock - Quantity);
    public decimal Subtotal => Math.Round(UnitPrice * Quantity, 2);
    public bool HasStockError => UnitsInStock > 0 && Quantity > UnitsInStock;
}