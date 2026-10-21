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
                    "Server=(localdb)\\mssqllocaldb;Database=NorthWindUsersDB" 
                }); 
        return new NorthWindMembershipContext(DbOptions); 
    } 
}
