using BackendServer.Data;
using Microsoft.AspNetCore.Mvc;

namespace BackendServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
	SimpleDb simpleDb;

	public TodoController()
	{
		simpleDb = new SimpleDb();
		simpleDb.InitAsync().Wait();
	}
    [Route("insertItem")]
    [HttpPost]
	public async Task<IActionResult> InsertItem([FromQuery] string userId, [FromQuery] string content)
	{
		var entity = new TodoEntity()
		{
			PartitionKey = $"todoitems;{userId}",
			Content = content,
			RowKey = Guid.NewGuid().ToString(),
			IsDone = false
		};

		await simpleDb.InsertAsync(entity);

		return Ok();
	}
    /*
    [Route("deleteItem")]
	[HttpDelete]
	public async Task<IActionResult> DeleteItem([FromQuery]string userId, [FromQuery] string rowKey, [FromQuery] string content, [FromQuery] bool isDone)
	{
        var entity = new TodoEntity()
        {
			PartitionKey = $"todoitems;{userId}",
            Content = content,
            RowKey = rowKey,
            IsDone = isDone
        };
		
		await simpleDb.DeleteAsync(entity);
		return Ok();
    }*/
    [Route("getItems")]
    [HttpGet]
	public async Task<IActionResult> GetItems([FromQuery]string userId)
	{
		var items = await simpleDb.GetEntitiesAsync<TodoEntity>($"todoitems;{userId}");
		return Ok(items.Select(item => new { id = item.RowKey, content = item.Content, isDone = item.IsDone}));
	}
    [Route("updateItem")]
    [HttpPut]
    public async Task<IActionResult> UpdateItem([FromQuery]string userId, [FromQuery] string rowKey, [FromQuery] string content, [FromQuery] bool isDone)
	{
        var entity = new TodoEntity()
        {
            PartitionKey = $"todoitems;{userId}",
            Content = content,
            RowKey = rowKey,
            IsDone = isDone
        };
        await simpleDb.UpdateAsync(entity);
		return Ok();
    }


}