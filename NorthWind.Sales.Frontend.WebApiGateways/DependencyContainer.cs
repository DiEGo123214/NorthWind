using NorthWind.HttpDelegatingHandlers;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using NorthWind.Sales.Frontend.WebApiGateways;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddWebApiGateways(
        this IServiceCollection services,
        Action<HttpClient> configureHttpClient,
        Action<IHttpClientBuilder> configureHttpClientBuilder)
    {
        services.AddExceptionDelegatingHandler();
        
        var createOrderBuilder = services
            .AddHttpClient<ICreateOrderGateway, CreateOrderGateway>(configureHttpClient)
            .AddHttpMessageHandler<ExceptionDelegatingHandler>();
        configureHttpClientBuilder(createOrderBuilder);

        var customersBuilder = services
            .AddHttpClient<ISearchCustomersGateway, SearchCustomersGateway>(configureHttpClient)
            .AddHttpMessageHandler<ExceptionDelegatingHandler>();
        configureHttpClientBuilder(customersBuilder);

        var productsBuilder = services
            .AddHttpClient<ISearchProductsGateway, SearchProductsGateway>(configureHttpClient)
            .AddHttpMessageHandler<ExceptionDelegatingHandler>();
        configureHttpClientBuilder(productsBuilder);

        var invoiceBuilder = services
            .AddHttpClient<IOrderInvoiceGateway, OrderInvoiceGateway>(configureHttpClient)
            .AddHttpMessageHandler<ExceptionDelegatingHandler>();
        configureHttpClientBuilder(invoiceBuilder);

        return services;
    }
}