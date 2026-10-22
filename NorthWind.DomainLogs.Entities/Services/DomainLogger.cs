using System.Transactions;

namespace NorthWind.DomainLogs.Entities.Services;

internal class DomainLogger(IDomainLogsRepository repository) : IDomainLogger
{
    public async Task LogInformation(DomainLog log)
    {
        using var scope = new TransactionScope(
            TransactionScopeOption.Suppress,
            TransactionScopeAsyncFlowOption.Enabled);

        await repository.Add(log);
        await repository.SaveChanges();
    }
}
