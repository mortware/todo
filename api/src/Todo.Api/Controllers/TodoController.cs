using Microsoft.AspNetCore.Mvc;
using Todo.Api.Requests;

namespace Todo.Api.Controllers;

[ApiController]
public class TodoController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly DbInitializer _dbInitialiser;

    public TodoController(IMediator mediator, DbInitializer dbInitialiser)
    {
        _mediator = mediator;
        _dbInitialiser = dbInitialiser;
    }

    [HttpGet("todo/list")]
    public async Task<IActionResult> List(CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new ListTodoItemsRequest(), cancellationToken));
    
    [HttpPost("todo/create")]
    public async Task<IActionResult> Create([FromBody] CreateTodoItemRequest request, CancellationToken cancellationToken) => Ok(await _mediator.Send(request, cancellationToken));

    [HttpGet("todo/reset")]
    public ActionResult Reset()
    {
        this._dbInitialiser.Seed();

        return this.NoContent();
    }
}