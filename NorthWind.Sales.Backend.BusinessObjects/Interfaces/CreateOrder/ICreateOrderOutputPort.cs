using NorthWind.Sales.Backend.BusinessObjects.Aggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.CreateOrder;

public interface ICreateOrderOutputPort
{
    int OrderId { get; }
    Task Handle(OrderAggregate addedOrder);
}