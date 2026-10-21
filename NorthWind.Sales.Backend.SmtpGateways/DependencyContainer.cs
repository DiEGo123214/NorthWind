using Microsoft.Extensions.DependencyInjection;
using NorthWind.Entities.Interfaces;
using NorthWind.Sales.Backend.SmtpGateways;
using NorthWind.Sales.Backend.SmtpGateways.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddMailServices(
        this IServiceCollection services,
        Action<SmtpOptions> configureSmtpOptions)
    {
        services.AddSingleton<IMailService, MailService>();
        services.Configure(configureSmtpOptions);
        return services;
    }
}
