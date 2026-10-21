using NorthWind.Sales.Entities.Dtos.Invoices;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.ValueObjects;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using System.Net.Http.Json;

namespace NorthWind.Sales.Frontend.WebApiGateways;

internal class OrderInvoiceGateway(HttpClient client) : IOrderInvoiceGateway
{
    public async Task<PaginatedResult<OrderSummaryDto>> SearchOrdersAsync(
        string? filter, 
        int pageIndex, 
        int pageSize, 
        string? searchField = null, 
        DateTime? fromDate = null, 
        DateTime? toDate = null)
    {
        var queryParams = new List<string>
        {
            $"pageIndex={pageIndex}",
            $"pageSize={pageSize}"
        };

        if (!string.IsNullOrWhiteSpace(filter))
            queryParams.Add($"filter={Uri.EscapeDataString(filter.Trim())}");

        if (!string.IsNullOrWhiteSpace(searchField))
            queryParams.Add($"searchField={Uri.EscapeDataString(searchField.Trim())}");

        if (fromDate.HasValue)
            queryParams.Add($"fromDate={fromDate.Value:yyyy-MM-dd}");

        if (toDate.HasValue)
            queryParams.Add($"toDate={toDate.Value:yyyy-MM-dd}");

        var url = $"{Endpoints.SearchOrders}?{string.Join("&", queryParams)}";
        var response = await client.GetFromJsonAsync<PaginatedResult<OrderSummaryDto>>(url);
        return response ?? new PaginatedResult<OrderSummaryDto>();
    }

    public async Task<OrderInvoiceDto?> GetOrderInvoiceAsync(int orderId)
    {
        var url = $"{Endpoints.GetOrderInvoice}/{orderId}";
        return await client.GetFromJsonAsync<OrderInvoiceDto>(url);
    }
}
