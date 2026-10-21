namespace NorthWind.Sales.Entities.Dtos.Products;

public record ProductDto(int Id, string Name, decimal UnitPrice, short UnitsInStock);
