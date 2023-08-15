using Azure.Data.Tables;

namespace FinopsSolution.API.Repository;

public abstract class BaseRepository<T> where T : class, ITableEntity
{
    private readonly string _connectionString;
    private readonly string _tableName;

    protected readonly TableClient _client;

    protected BaseRepository(string connectionString, string tableName)
    {
        _connectionString = connectionString;
        _tableName = tableName;

        _client = new TableClient(connectionString, tableName);
    }

    public async Task InitializeAsync()
    {
        TableServiceClient tableServiceClient = new TableServiceClient(_connectionString);
        await tableServiceClient.CreateTableIfNotExistsAsync(_tableName);
    }

    public async Task UpsertAsync(IEnumerable<T> entities)
    {
        IEnumerable<IGrouping<string, T>> groups = entities.GroupBy(x => x.PartitionKey);

        foreach (IGrouping<string, T> group in groups)
        {
            foreach (T[] chunk in group.Chunk(100))
            {
                // Create the batch.
                List<TableTransactionAction> upsertEntitiesBatch = new();

                // Add the entities to be added to the batch.
                upsertEntitiesBatch.AddRange(chunk.Select(e => new TableTransactionAction(TableTransactionActionType.UpsertMerge, e)));

                // Submit the batch.
                _ = await _client.SubmitTransactionAsync(upsertEntitiesBatch).ConfigureAwait(false);
            }
        }
    }

    public async Task InsertSubscriptionCostToTable(T entity)
    {
        _ = await _client.UpsertEntityAsync(entity);
    }
}