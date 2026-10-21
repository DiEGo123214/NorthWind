namespace NorthWind.Sales.Entities.Dtos.Customers;

public record CustomerDto(
    string Id, 
    string Name, 
    decimal CurrentBalance,
    string? Address = null,
    string? City = null,
    string? Country = null,
    string? PostalCode = null,
    string? Email = null);
