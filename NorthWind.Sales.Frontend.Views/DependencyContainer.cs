using NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddViewsServices(this IServiceCollection services)
    {
        services.AddScoped<CreateOrderViewModel>();
        services.AddModelValidator<CreateOrderViewModel, CreateOrderViewModelValidator>();
        return services;
    }
}