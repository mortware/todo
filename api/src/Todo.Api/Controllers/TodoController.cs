using Microsoft.AspNetCore.Mvc;
using Todo.Api.Requests;

namespace Todo.Api.Controllers;

[ApiController]
public class TodoController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("todo/list")]
    public async Task<IActionResult> List(CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new ListTodoItemsRequest(), cancellationToken));
    
    [HttpPost("todo/create")]
    public async Task<IActionResult> Get([FromBody] CreateTodoItemRequest request, CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(request, cancellationToken));
}