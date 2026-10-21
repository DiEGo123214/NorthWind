using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos.Customers;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.ValueObjects;

namespace Microsoft.AspNetCore.Builder;

public static class SearchCustomersController
{
    public static WebApplication UseSearchCustomersController(this WebApplication app)
    {
        app.MapGet(Endpoints.SearchCustomers, SearchCustomers)
            .RequireAuthorization();
        app.MapGet("/api/customers/{id}", GetCustomerById)
            .RequireAuthorization();
        app.MapPut("/api/customers/{id}/location", UpdateCustomerLocation)
            .RequireAuthorization();
        return app;
    }

    public static async Task<IResult> SearchCustomers(
        [FromQuery] string? filter,
        [FromQuery] int pageIndex,
        [FromQuery] int pageSize,
        [FromQuery] string? searchField,
        IQueriesRepository repository)
    {
        if (pageIndex <= 0) pageIndex = 1;
        if (pageSize <= 0) pageSize = 10;

        var result = await repository.SearchCustomers(filter, pageIndex, pageSize, searchField);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetCustomerById(
        string id,
        IQueriesRepository repository)
    {
        var customer = await repository.GetCustomerById(id);
        return customer != null ? Results.Ok(customer) : Results.NotFound();
    }

    public static async Task<IResult> UpdateCustomerLocation(
        string id,
        [FromBody] UpdateCustomerLocationDto dto,
        ICommandsRepository commandsRepository,
        IQueriesRepository repository)
    {
        await commandsRepository.UpdateCustomerLocation(id, dto.Address, dto.City, dto.Country, dto.PostalCode);
        await commandsRepository.SaveChanges();
        var updated = await repository.GetCustomerById(id);
        return Results.Ok(updated);
    }
}
