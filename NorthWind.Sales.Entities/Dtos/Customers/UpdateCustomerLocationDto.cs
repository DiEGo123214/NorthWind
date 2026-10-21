namespace NorthWind.Sales.Entities.Dtos.Customers;

public record UpdateCustomerLocationDto(
    string CustomerId,
    string? Address,
    string? City,
    string? Country,
    string? PostalCode);
