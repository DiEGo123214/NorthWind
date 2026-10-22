namespace NorthWind.Membership.Backend.AspNetIdentity.DataContexts;

internal class NorthWindMembershipContextFactory : 
    IDesignTimeDbContextFactory<NorthWindMembershipContext> 
{ 
    public NorthWindMembershipContext CreateDbContext(string[] args) 
    { 
        IOptions<MembershipDBOptions> DbOptions = 
            Microsoft.Extensions.Options.Options.Create( 
                new MembershipDBOptions() 
                { 
                    ConnectionString = 
                    "Server=10.0.2.2,1433;Database=NorthWindUsersDB;User Id=sa;Password=DIEgo344*;TrustServerCertificate=True" 
                }); 
        return new NorthWindMembershipContext(DbOptions); 
    } 
}
