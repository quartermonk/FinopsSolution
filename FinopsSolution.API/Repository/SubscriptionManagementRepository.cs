using Azure.Data.Tables;
using FinopsSolution.API.Models;
using FinopsSolution.API.Repository.Entities;
using Microsoft.Extensions.Options;

namespace FinopsSolution.API.Repository;

public class SubscriptionManagementRepository : BaseRepository<SubscriptionEntity>
{
    public SubscriptionManagementRepository(IOptions<TableStorageSettings> options)
        : base(options.Value.ConnectionString, options.Value.TableNames.SubscriptionDetails)
    {
    }
}
