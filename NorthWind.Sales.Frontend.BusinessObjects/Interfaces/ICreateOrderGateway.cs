using NorthWind.Sales.Entities.Dtos.CreateOrder;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface ICreateOrderGateway
{
    Task<int> CreateOrderAsync(CreateOrderDto order);
}