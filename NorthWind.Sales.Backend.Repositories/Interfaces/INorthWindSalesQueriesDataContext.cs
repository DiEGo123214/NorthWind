using NorthWind.Sales.Backend.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Backend.Repositories.Interfaces;

public interface INorthWindSalesQueriesDataContext
{
    IQueryable<Customer> Customers { get; }
    IQueryable<Product> Products { get; }
    IQueryable<NorthWind.Sales.Backend.BusinessObjects.POCOEntities.Order> Orders { get; }
    IQueryable<OrderDetail> OrderDetails { get; }
    Task<ReturnType> FirstOrDefaultAync<ReturnType>(IQueryable<ReturnType> queryable);
    Task<IEnumerable<ReturnType>> ToListAsync<ReturnType>(IQueryable<ReturnType> queryable);
    Task<int> CountAsync<ReturnType>(IQueryable<ReturnType> queryable);
}