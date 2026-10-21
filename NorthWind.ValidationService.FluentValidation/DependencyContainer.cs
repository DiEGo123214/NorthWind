using NorthWind.Validation.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddValidationService(
        this IServiceCollection services)
    {
        services.AddScoped(typeof(IValidationService<>),
            typeof(FluentValidationService<>));
        return services;
    }
}