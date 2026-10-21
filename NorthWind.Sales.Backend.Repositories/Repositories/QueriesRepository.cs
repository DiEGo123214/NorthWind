using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Backend.BusinessObjects.ValueObjects;
using NorthWind.Sales.Backend.Repositories.Interfaces;
using NorthWind.Sales.Entities.Dtos.Customers;
using NorthWind.Sales.Entities.Dtos.Invoices;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Backend.Repositories.Repositories;

internal class QueriesRepository(INorthWindSalesQueriesDataContext context) : IQueriesRepository
{
    public async Task<decimal?> GetCustomerCurrentBalance(string customerId)
    {
        var Queryable = context.Customers
            .Where(c => c.Id == customerId)
            .Select(c => new { c.CurrentBalance });

        var Result = await context.FirstOrDefaultAync(Queryable);
        return Result?.CurrentBalance;
    }

    public async Task<CustomerDto?> GetCustomerById(string customerId)
    {
        var query = context.Customers
            .Where(c => c.Id == customerId)
            .Select(c => new CustomerDto(c.Id, c.Name, c.CurrentBalance, c.Address, c.City, c.Country, c.PostalCode, c.Email));

        return await context.FirstOrDefaultAync(query);
    }

    public async Task<IEnumerable<ProductUnitsInStock>> GetProductsUnitsInStock(IEnumerable<int> productIds)
    {
        var Queryable = context.Products
            .Where(p => productIds.Contains(p.Id))
            .Select(p => new ProductUnitsInStock(p.Id, p.UnitsInStock));

        return await context.ToListAsync(Queryable);
    }

    public async Task<PaginatedResult<CustomerDto>> SearchCustomers(string? filter, int pageIndex, int pageSize, string? searchField = null)
    {
        pageIndex = Math.Max(1, pageIndex);
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var query = context.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            var term = filter.Trim();
            switch (searchField?.ToLowerInvariant())
            {
                case "customerid":
                case "id":
                case "codigo":
                    query = query.Where(c => c.Id.Contains(term));
                    break;
                case "customername":
                case "name":
                case "nombre":
                    query = query.Where(c => c.Name.Contains(term));
                    break;
                default:
                    query = query.Where(c => c.Id.Contains(term) || c.Name.Contains(term));
                    break;
            }
        }

        int totalCount = await context.CountAsync(query);

        var pagedQuery = query
            .OrderBy(c => c.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CustomerDto(c.Id, c.Name, c.CurrentBalance, c.Address, c.City, c.Country, c.PostalCode, c.Email));

        var items = await context.ToListAsync(pagedQuery);

        return new PaginatedResult<CustomerDto>(items.ToList(), totalCount, pageIndex, pageSize);
    }

    public async Task<PaginatedResult<ProductDto>> SearchProducts(string? filter, int pageIndex, int pageSize, string? searchField = null)
    {
        pageIndex = Math.Max(1, pageIndex);
        pageSize = pageSize <= 0 ? 10 : pageSize;

        // Requisito 8: SOLO productos con stock mayor que cero
        var query = context.Products.Where(p => p.UnitsInStock > 0);

        if (!string.IsNullOrWhiteSpace(filter))
        {
            var term = filter.Trim();
            bool isNumber = int.TryParse(term, out int searchId);

            switch (searchField?.ToLowerInvariant())
            {
                case "productid":
                case "id":
                case "codigo":
                    query = isNumber ? query.Where(p => p.Id == searchId) : query.Where(p => p.Id.ToString().Contains(term));
                    break;
                case "productname":
                case "name":
                case "nombre":
                    query = query.Where(p => p.Name.Contains(term));
                    break;
                default:
                    query = isNumber 
                        ? query.Where(p => p.Id == searchId || p.Name.Contains(term)) 
                        : query.Where(p => p.Name.Contains(term));
                    break;
            }
        }

        int totalCount = await context.CountAsync(query);

        var pagedQuery = query
            .OrderBy(p => p.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto(p.Id, p.Name, p.UnitPrice, p.UnitsInStock));

        var items = await context.ToListAsync(pagedQuery);

        return new PaginatedResult<ProductDto>(items.ToList(), totalCount, pageIndex, pageSize);
    }

    public async Task<PaginatedResult<OrderSummaryDto>> SearchOrders(
        string? filter, 
        int pageIndex, 
        int pageSize, 
        string? searchField = null, 
        DateTime? fromDate = null, 
        DateTime? toDate = null)
    {
        pageIndex = Math.Max(1, pageIndex);
        pageSize = pageSize <= 0 ? 5 : pageSize;

        var ordersQuery = context.Orders.AsQueryable();
        var customersQuery = context.Customers.AsQueryable();

        var query = from o in ordersQuery
                    join c in customersQuery on o.CustomerId equals c.Id into custJoin
                    from cust in custJoin.DefaultIfEmpty()
                    select new
                    {
                        o.Id,
                        o.CustomerId,
                        CustomerName = cust != null ? cust.Name : o.CustomerId,
                        o.OrderDate,
                        o.ShipCountry,
                        o.ShipCity
                    };

        if (!string.IsNullOrWhiteSpace(filter))
        {
            var term = filter.Trim();
            bool isNumber = int.TryParse(term, out int searchId);

            switch (searchField?.ToLowerInvariant())
            {
                case "invoicenumber":
                case "id":
                case "numero":
                    query = isNumber ? query.Where(q => q.Id == searchId) : query.Where(q => q.Id.ToString().Contains(term));
                    break;
                case "customerid":
                case "codigo":
                    query = query.Where(q => q.CustomerId.Contains(term));
                    break;
                case "customername":
                case "nombre":
                    query = query.Where(q => q.CustomerName.Contains(term));
                    break;
                default:
                    query = isNumber 
                        ? query.Where(q => q.Id == searchId || q.CustomerId.Contains(term) || q.CustomerName.Contains(term))
                        : query.Where(q => q.CustomerId.Contains(term) 
                                           || q.CustomerName.Contains(term)
                                           || (q.ShipCountry != null && q.ShipCountry.Contains(term))
                                           || (q.ShipCity != null && q.ShipCity.Contains(term)));
                    break;
            }
        }

        if (fromDate.HasValue)
        {
            var from = fromDate.Value.Date;
            query = query.Where(q => q.OrderDate >= from);
        }

        if (toDate.HasValue)
        {
            var to = toDate.Value.Date.AddDays(1);
            query = query.Where(q => q.OrderDate < to);
        }

        int totalCount = await context.CountAsync(query);

        var paged = query
            .OrderByDescending(o => o.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);

        var pagedList = (await context.ToListAsync(paged)).ToList();

        if (pagedList.Count == 0)
        {
            return new PaginatedResult<OrderSummaryDto>(new List<OrderSummaryDto>(), totalCount, pageIndex, pageSize);
        }

        // Optimización clave: traer los detalles de TODAS las órdenes paginadas en una única consulta SQL (eliminando el bucle N+1)
        var orderIds = pagedList.Select(o => o.Id).ToList();
        var allDetailsQuery = context.OrderDetails
            .Where(d => orderIds.Contains(d.OrderId))
            .Select(d => new { d.OrderId, d.UnitPrice, d.Quantity });

        var allDetails = await context.ToListAsync(allDetailsQuery);
        var detailsByOrder = allDetails.GroupBy(d => d.OrderId)
                                       .ToDictionary(g => g.Key, g => g.ToList());

        var resultList = new List<OrderSummaryDto>(pagedList.Count);
        foreach (var item in pagedList)
        {
            if (detailsByOrder.TryGetValue(item.Id, out var details))
            {
                decimal subtotal = details.Sum(d => (decimal)d.Quantity * d.UnitPrice);
                decimal total = Math.Round(subtotal * 1.15m, 2);

                resultList.Add(new OrderSummaryDto
                {
                    Id = item.Id,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    ShipCountry = item.ShipCountry ?? string.Empty,
                    ShipCity = item.ShipCity ?? string.Empty,
                    OrderDate = item.OrderDate,
                    ItemCount = details.Count,
                    Total = total
                });
            }
            else
            {
                resultList.Add(new OrderSummaryDto
                {
                    Id = item.Id,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    ShipCountry = item.ShipCountry ?? string.Empty,
                    ShipCity = item.ShipCity ?? string.Empty,
                    OrderDate = item.OrderDate,
                    ItemCount = 0,
                    Total = 0
                });
            }
        }

        return new PaginatedResult<OrderSummaryDto>(resultList, totalCount, pageIndex, pageSize);
    }

    public async Task<OrderInvoiceDto?> GetOrderInvoice(int orderId)
    {
        var order = await context.FirstOrDefaultAync(
            context.Orders.Where(o => o.Id == orderId));

        if (order == null) return null;

        var customer = await context.FirstOrDefaultAync(
            context.Customers.Where(c => c.Id == order.CustomerId));

        var detailsQuery = from d in context.OrderDetails.Where(d => d.OrderId == orderId)
                           join p in context.Products on d.ProductId equals p.Id into prodJoin
                           from prod in prodJoin.DefaultIfEmpty()
                           select new OrderInvoiceDetailDto
                           {
                               ProductId = d.ProductId,
                               ProductName = prod != null ? prod.Name : $"Producto #{d.ProductId}",
                               UnitPrice = d.UnitPrice,
                               Quantity = d.Quantity
                           };

        var details = await context.ToListAsync(detailsQuery);

        return new OrderInvoiceDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CustomerName = customer?.Name ?? order.CustomerId,
            OrderDate = order.OrderDate,
            ShipAddress = order.ShipAddress,
            ShipCity = order.ShipCity,
            ShipCountry = order.ShipCountry,
            ShipPostalCode = order.ShipPostalCode,
            Details = details.ToList()
        };
    }
}