using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Commands.CreateToDo;
using ToDoApp.Application.Queries.ToDoItem;

namespace ToDoApp.API.Controllers;

[Route("api/todoitem")]
[ApiController]
public class ToDoItemController : ControllerBase
{
    private readonly ISender _sender;

    public ToDoItemController(ISender sender) => _sender = sender;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var result = await _sender.Send(new ToDoItemQuery());  
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] CreateToDoItemCommand command)
    {
        await _sender.Send(command);

        return StatusCode(201);
    }
}
