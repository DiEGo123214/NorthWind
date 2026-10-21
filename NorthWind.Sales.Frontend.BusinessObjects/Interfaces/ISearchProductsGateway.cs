using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.Dtos.Products;

namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface ISearchProductsGateway
{
    Task<PaginatedResult<ProductDto>> SearchProductsAsync(string? filter, int pageIndex, int pageSize, string? searchField = null);
}
