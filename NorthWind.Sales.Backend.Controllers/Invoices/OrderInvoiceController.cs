using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos.Invoices;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.ValueObjects;

namespace Microsoft.AspNetCore.Builder;

public static class OrderInvoiceController
{
    public static WebApplication UseOrderInvoiceController(this WebApplication app)
    {
        app.MapGet(Endpoints.SearchOrders, SearchOrders)
            .RequireAuthorization();

        app.MapGet($"{Endpoints.GetOrderInvoice}/{{id:int}}", GetOrderInvoice)
            .RequireAuthorization();

        return app;
    }

    public static async Task<IResult> SearchOrders(
        [FromQuery] string? filter,
        [FromQuery] string? searchField,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int pageIndex,
        [FromQuery] int pageSize,
        IQueriesRepository repository)
    {
        if (pageIndex <= 0) pageIndex = 1;
        if (pageSize <= 0) pageSize = 5;

        var result = await repository.SearchOrders(filter, pageIndex, pageSize, searchField, fromDate, toDate);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetOrderInvoice(
        int id,
        IQueriesRepository repository)
    {
        var invoice = await repository.GetOrderInvoice(id);
        if (invoice == null)
        {
            return Results.NotFound($"Order #{id} was not found.");
        }
        return Results.Ok(invoice);
    }
}
