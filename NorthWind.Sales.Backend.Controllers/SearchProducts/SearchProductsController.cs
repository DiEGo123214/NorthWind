using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.Dtos.Products;
using NorthWind.Sales.Entities.ValueObjects;

namespace Microsoft.AspNetCore.Builder;

public static class SearchProductsController
{
    public static WebApplication UseSearchProductsController(this WebApplication app)
    {
        app.MapGet(Endpoints.SearchProducts, SearchProducts)
            .RequireAuthorization();
        return app;
    }

    public static async Task<IResult> SearchProducts(
        [FromQuery] string? filter,
        [FromQuery] int pageIndex,
        [FromQuery] int pageSize,
        [FromQuery] string? searchField,
        IQueriesRepository repository)
    {
        if (pageIndex <= 0) pageIndex = 1;
        if (pageSize <= 0) pageSize = 10;

        var result = await repository.SearchProducts(filter, pageIndex, pageSize, searchField);
        return Results.Ok(result);
    }
}
