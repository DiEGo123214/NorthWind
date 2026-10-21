using NorthWind.Sales.Entities.Dtos.Customers;
using NorthWind.Sales.Entities.Dtos.Pagination;

namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface ISearchCustomersGateway
{
    Task<PaginatedResult<CustomerDto>> SearchCustomersAsync(string? filter, int pageIndex, int pageSize, string? searchField = null);
    Task<CustomerDto?> GetCustomerByIdAsync(string customerId);
    Task<CustomerDto?> UpdateCustomerLocationAsync(UpdateCustomerLocationDto dto);
}
