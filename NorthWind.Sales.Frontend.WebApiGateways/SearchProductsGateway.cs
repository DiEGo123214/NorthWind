using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.Dtos.Products;
using NorthWind.Sales.Entities.ValueObjects;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using System.Net.Http.Json;

namespace NorthWind.Sales.Frontend.WebApiGateways;

internal class SearchProductsGateway(HttpClient client) : ISearchProductsGateway
{
    public async Task<PaginatedResult<ProductDto>> SearchProductsAsync(string? filter, int pageIndex, int pageSize, string? searchField = null)
    {
        var url = $"{Endpoints.SearchProducts}?filter={Uri.EscapeDataString(filter ?? "")}&pageIndex={pageIndex}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(searchField))
        {
            url += $"&searchField={Uri.EscapeDataString(searchField)}";
        }
        var response = await client.GetFromJsonAsync<PaginatedResult<ProductDto>>(url);
        return response ?? new PaginatedResult<ProductDto>();
    }
}
