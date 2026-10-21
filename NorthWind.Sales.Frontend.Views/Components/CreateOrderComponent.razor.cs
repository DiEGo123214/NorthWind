using Microsoft.AspNetCore.Components;
using NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Frontend.Views.Components;

public partial class CreateOrderComponent
{
    [Parameter]
    public CreateOrderViewModel Order { get; set; }
}