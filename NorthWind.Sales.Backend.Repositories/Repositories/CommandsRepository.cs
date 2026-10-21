using NorthWind.Sales.Backend.BusinessObjects.Aggregates;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Backend.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Backend.Repositories.Repositories;

internal class CommandsRepository(INorthWindSalesCommandsDataContext context) : ICommandsRepository
{
    public async Task CreateOrder(OrderAggregate order)
    {
        await context.AddOrderAsync(order);
        await context.AddOrderDetailsAsync(
            order.OrderDetails
            .Select(d => new Entities.OrderDetail
            {
                Order = order,
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice
            }).ToArray());

        foreach (var detail in order.OrderDetails)
        {
            await context.DecreaseStockAsync(detail.ProductId, detail.Quantity);
        }
    }

    public async Task UpdateCustomerLocation(string customerId, string? address, string? city, string? country, string? postalCode)
    {
        await context.UpdateCustomerLocationAsync(customerId, address, city, country, postalCode);
    }

    public async Task SaveChanges() =>
        await context.SaveChangesAsync();
}