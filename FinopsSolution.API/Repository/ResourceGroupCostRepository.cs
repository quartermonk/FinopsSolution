using FinopsSolution.API.Repository.Entities;
using Microsoft.Extensions.Options;

namespace FinopsSolution.API.Repository;

public class ResourceGroupCostRepository : BaseRepository<ResourceGroupEntity>
{
    public ResourceGroupCostRepository(IOptions<TableStorageSettings> options)
        : base(options.Value.ConnectionString, options.Value.TableNames.ResourceGroupDetails)
    {
    }
}
