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
                        "Server=localhost;Database=NorthWindLogsDB;User Id=sa;Password=DIEgo344;TrustServerCertificate=True"
                });
        return new NorthWindDomainLogsContext(DbOptions);
    }
}
