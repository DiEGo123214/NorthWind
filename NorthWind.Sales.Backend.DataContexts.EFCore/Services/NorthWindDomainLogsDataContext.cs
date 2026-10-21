using NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;
using NorthWind.Sales.Backend.DataContexts.EFCore.Guards;
using NorthWind.Sales.Backend.Repositories.Interfaces;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;

internal class NorthWindDomainLogsDataContext(IOptions<DBOptions> dbOptions) :
    NorthWindDomainLogsContext(dbOptions),
    INorthWindDomainLogsDataContext
{
    public async Task AddLogAsync(DomainLog log) =>
        await AddAsync(log);

    public async Task SaveChangesAsync() =>
        await GuardDBContext.AgainstSaveChangesErrorAsync(this);
}
