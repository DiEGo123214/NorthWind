using NorthWind.Sales.Entities.Dtos.Invoices;
using NorthWind.Sales.Entities.Dtos.Pagination;

namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface IOrderInvoiceGateway
{
    Task<PaginatedResult<OrderSummaryDto>> SearchOrdersAsync(
        string? filter, 
        int pageIndex, 
        int pageSize, 
        string? searchField = null, 
        DateTime? fromDate = null, 
        DateTime? toDate = null);
    Task<OrderInvoiceDto?> GetOrderInvoiceAsync(int orderId);
}
