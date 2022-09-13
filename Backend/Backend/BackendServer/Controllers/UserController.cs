using BackendServer.Data;
using Microsoft.AspNetCore.Mvc;

namespace BackendServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    SimpleDb simpleDb;

    public UserController()
    {
        simpleDb = new SimpleDb();
        simpleDb.InitAsync().Wait();
    }
    [Route("insertUser")]
    [HttpPost]
    public async Task<IActionResult> InsertUser([FromQuery] string userEmail, [FromQuery] string userName)
    {
        var entity = new UserEntity()
        {
            PartitionKey = $"user={userName}",
            RowKey = Guid.NewGuid().ToString(),
            UserEmail = userEmail,
            UserName = userName,
        };

        await simpleDb.InsertAsync(entity);

        return Ok();
    }
    [Route("getUser")]
    [HttpGet]
    public async Task<IActionResult> GetUser([FromQuery] string userEmail, [FromQuery] string userName)
    {
        var users = await simpleDb.GetEntitiesAsync<UserEntity>($"user={userName}");
        return Ok(users.Select(user => new { id = user.PartitionKey, rowKey = user.RowKey,  userName = user.UserName, userEmail = user.UserEmail }));
    }
    /*
    [Route("deleteUser")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromQuery] string rowKey, [FromQuery] string userEmail, [FromQuery] string userName)
    {
        var entity = new UserEntity()
        {
            PartitionKey = $"user={userName}",
            RowKey = rowKey,
            UserEmail = userEmail,
            UserName = userName,
        };

        await simpleDb.DeleteAsync(entity);
        return Ok();
    }*/
}
