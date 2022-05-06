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
    public async Task<IActionResult> List() =>
        Ok(await _mediator.Send(new ListTodoItemsRequest()));
    
    [HttpPost("todo/create")]
    public async Task<IActionResult> Get([FromBody] CreateTodoItemRequest request) =>
        Ok(await _mediator.Send(request));



    [HttpPost("todo/update")]
    public async Task<IActionResult> Update([FromBody] UpdateTodoItemRequest request) =>
        Ok(await _mediator.Send(request));

}