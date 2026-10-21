using NorthWind.Sales.Backend.BusinessObjects.POCOEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Backend.Repositories.Interfaces;

public interface INorthWindSalesCommandsDataContext
{
    Task AddOrderAsync(Order order);
    Task AddOrderDetailsAsync(IEnumerable<Entities.OrderDetail> orderDetails);
    Task DecreaseStockAsync(int productId, short quantity);
    Task UpdateCustomerLocationAsync(string customerId, string? address, string? city, string? country, string? postalCode);
    Task SaveChangesAsync();
}