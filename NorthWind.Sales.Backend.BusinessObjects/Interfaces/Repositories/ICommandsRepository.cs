using NorthWind.Sales.Backend.BusinessObjects.Aggregates;
using NorthWind.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;

public interface ICommandsRepository : IUnitOfWork
{
    Task CreateOrder(OrderAggregate order);
    Task UpdateCustomerLocation(string customerId, string? address, string? city, string? country, string? postalCode);
}