using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Cosmos.Table.Protocol;

public class SimpleDb
{
    private CloudTable _table;
    const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=climsystemsinterviewdemo;AccountKey=l+s0O3exaYztOdnI0WoUDeAWUpCYlGs4fK7UgbZcJt1dRDECrej5fIEvGWx0V5W3+MoAybXIxoC0+AStRfnJFA==;EndpointSuffix=core.windows.net";
    const string TableName = "mydata";
    
    public async Task InitAsync()
    {
        var account = CloudStorageAccount.Parse(ConnectionString);
        var client = account.CreateCloudTableClient();
        var table = client.GetTableReference(TableName);
        if (!await table.ExistsAsync())
            await table.CreateAsync();
        _table = table;         
    }
    
    public async Task InsertAsync(TableEntity entity)
    {
        await _table.ExecuteAsync(TableOperation.Insert(entity));
    }
    /*
    public async Task DeleteAsync(TableEntity entity)
    {
        await _table.ExecuteAsync(TableOperation.Delete(entity));
    }
    */
    public async Task UpdateAsync(TableEntity entity)
    {
        await _table.ExecuteAsync(TableOperation.InsertOrReplace(entity));
    }
    
    public async Task<T?> GetEntityAsync<T>(string pk, string rk)
        where T: TableEntity, new()
    {
        var result = await _table.ExecuteAsync(TableOperation.Retrieve<T>(pk, rk));
        return (T)result.Result;
    }
    
    public async Task<IEnumerable<T>?> GetEntitiesAsync<T>(string pk)
        where T: TableEntity, new()
    {
        var query = new TableQuery<T>()
            .Where(TableQuery.GenerateFilterCondition(TableConstants.PartitionKey, QueryComparisons.Equal, pk));
        
        var results = await Task.Run(() => _table.ExecuteQuery(query));
        
        return results;
    }
}