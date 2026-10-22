namespace NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

internal class NorthWindDomainLogsContextFactory :
    IDesignTimeDbContextFactory<NorthWindDomainLogsContext>
{
    public NorthWindDomainLogsContext CreateDbContext(string[] args)
    {
        IOptions<DBOptions> DbOptions =
            Microsoft.Extensions.Options.Options.Create(
                new DBOptions
                {
                    DomainLogsConnectionString =
                        "Server=10.0.2.2,1433;Database=NorthWindLogsDB;User Id=sa;Password=DIEgo344*;TrustServerCertificate=True"
                });
        return new NorthWindDomainLogsContext(DbOptions);
    }
}
