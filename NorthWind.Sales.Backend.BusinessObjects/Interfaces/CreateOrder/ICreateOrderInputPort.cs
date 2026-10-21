using NorthWind.Sales.Entities.Dtos.CreateOrder;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.CreateOrder;

public interface ICreateOrderInputPort
{
    Task Handle(CreateOrderDto orderDto);
}