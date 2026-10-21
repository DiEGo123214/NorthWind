using NorthWind.Sales.Entities.Dtos.Customers;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.ValueObjects;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using System.Net.Http.Json;

namespace NorthWind.Sales.Frontend.WebApiGateways;

internal class SearchCustomersGateway(HttpClient client) : ISearchCustomersGateway
{
    public async Task<PaginatedResult<CustomerDto>> SearchCustomersAsync(string? filter, int pageIndex, int pageSize, string? searchField = null)
    {
        var url = $"{Endpoints.SearchCustomers}?filter={Uri.EscapeDataString(filter ?? "")}&pageIndex={pageIndex}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(searchField))
        {
            url += $"&searchField={Uri.EscapeDataString(searchField)}";
        }
        var response = await client.GetFromJsonAsync<PaginatedResult<CustomerDto>>(url);
        return response ?? new PaginatedResult<CustomerDto>();
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(string customerId)
    {
        try
        {
            var url = $"/api/customers/{Uri.EscapeDataString(customerId)}";
            return await client.GetFromJsonAsync<CustomerDto>(url);
        }
        catch
        {
            return null;
        }
    }

    public async Task<CustomerDto?> UpdateCustomerLocationAsync(UpdateCustomerLocationDto dto)
    {
        var url = $"/api/customers/{Uri.EscapeDataString(dto.CustomerId)}/location";
        var response = await client.PutAsJsonAsync(url, dto);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CustomerDto>();
        }
        return null;
    }
}
