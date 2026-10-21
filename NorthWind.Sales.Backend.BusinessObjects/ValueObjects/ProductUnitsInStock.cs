using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Backend.BusinessObjects.ValueObjects;

public class ProductUnitsInStock(int productId, short unitsInStock)
{
    public int ProductId => productId;
    public short UnitsInStock => unitsInStock;
}