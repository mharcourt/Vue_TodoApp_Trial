using Microsoft.Azure.Cosmos.Table;

namespace BackendServer.Data
{
    public class TodoEntity : TableEntity
    {
        public bool IsDone { get; set; }
        public string Content { get; set; }
    }
}
