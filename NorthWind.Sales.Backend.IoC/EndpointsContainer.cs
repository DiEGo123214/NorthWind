namespace Microsoft.AspNetCore.Builder;

public static class EndpointsContainer
{
    public static WebApplication MapNorthWindSalesEndpoints(
        this WebApplication app)
    {
        app.UseCreateOrderController();
        app.UseSearchCustomersController();
        app.UseSearchProductsController();
        app.UseOrderInvoiceController();
        app.UseMembershipEndpoints();
        return app;
    }
}
