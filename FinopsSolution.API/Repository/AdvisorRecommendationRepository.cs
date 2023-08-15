using FinopsSolution.API.Repository.Entities;
using Microsoft.Extensions.Options;

namespace FinopsSolution.API.Repository;

public class AdvisorRecommendationRepository : BaseRepository<AdvisorRecommendationEntity>
{
    public AdvisorRecommendationRepository(IOptions<TableStorageSettings> options)
        : base(options.Value.ConnectionString, options.Value.TableNames.AdvisorRecommendation)
    {
    }
}
