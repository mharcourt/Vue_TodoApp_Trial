using Microsoft.Azure.Cosmos.Table;

namespace BackendServer.Data
{
    public class UserEntity : TableEntity
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}
