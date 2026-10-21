using NorthWind.Sales.Backend.BusinessObjects.ValueObjects;
using NorthWind.Sales.Entities.Dtos.Customers;
using NorthWind.Sales.Entities.Dtos.Invoices;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;

public interface IQueriesRepository
{
    Task<decimal?> GetCustomerCurrentBalance(string customerId);
    Task<CustomerDto?> GetCustomerById(string customerId);
    Task<IEnumerable<ProductUnitsInStock>> GetProductsUnitsInStock(IEnumerable<int> productIds);
    Task<PaginatedResult<CustomerDto>> SearchCustomers(string? filter, int pageIndex, int pageSize, string? searchField = null);
    Task<PaginatedResult<ProductDto>> SearchProducts(string? filter, int pageIndex, int pageSize, string? searchField = null);
    Task<PaginatedResult<OrderSummaryDto>> SearchOrders(
        string? filter, 
        int pageIndex, 
        int pageSize, 
        string? searchField = null, 
        DateTime? fromDate = null, 
        DateTime? toDate = null);
    Task<OrderInvoiceDto?> GetOrderInvoice(int orderId);
}