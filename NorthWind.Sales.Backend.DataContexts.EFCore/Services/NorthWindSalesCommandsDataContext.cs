using Microsoft.Extensions.Options;
using NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;
using NorthWind.Sales.Backend.DataContexts.EFCore.Guards;
using NorthWind.Sales.Backend.DataContexts.EFCore.Options;
using NorthWind.Sales.Backend.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;

internal class NorthWindSalesCommandsDataContext(
    IOptions<DBOptions> dbOptions) :
    NorthWindSalesContext(dbOptions),
    INorthWindSalesCommandsDataContext
{
    public async Task AddOrderAsync(Order order) =>
        await AddAsync(order);

    public async Task AddOrderDetailsAsync(
        IEnumerable<Repositories.Entities.OrderDetail> orderDetails) =>
        await AddRangeAsync(orderDetails);

    public async Task DecreaseStockAsync(int productId, short quantity)
    {
        var product = await Products.FindAsync(productId);
        if (product != null)
        {
            if (product.UnitsInStock < quantity)
            {
                throw new InvalidOperationException(
                    $"Insufficient stock for product '{product.Name}'. Current stock: {product.UnitsInStock}, requested: {quantity}");
            }
            product.UnitsInStock = (short)(product.UnitsInStock - quantity);
        }
    }

    public async Task UpdateCustomerLocationAsync(string customerId, string? address, string? city, string? country, string? postalCode)
    {
        var customer = await Customers.FindAsync(customerId);
        if (customer != null)
        {
            customer.Address = address;
            customer.City = city;
            customer.Country = country;
            customer.PostalCode = postalCode;
        }
    }

    public async Task SaveChangesAsync() =>
        await GuardDBContext.AgainstSaveChangesErrorAsync(this);
}