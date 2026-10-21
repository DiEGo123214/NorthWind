using NorthWind.Sales.Entities; // Por si Endpoints está aquí o en otro subespacio de nombres
using NorthWind.Sales.Entities.Dtos.CreateOrder; // <- Aquí está el espacio de nombres real
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using NorthWind.Sales.Entities.ValueObjects;
using System.Net.Http.Json;

namespace NorthWind.Sales.Frontend.WebApiGateways;

internal class CreateOrderGateway(HttpClient client) : ICreateOrderGateway
{
    public async Task<int> CreateOrderAsync(CreateOrderDto order)
    {
        var Response = await client.PostAsJsonAsync(Endpoints.CreateOrder, order);
        return await Response.Content.ReadFromJsonAsync<int>();
    }
}