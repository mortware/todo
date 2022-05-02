using Todo.Api.Requests;

namespace Todo.Api.Handlers;

public class CreateTodoItemHandler : IRequestHandler<CreateTodoItemRequest, Guid>
{
    private readonly ITodoRepository _todoRepository;

    public CreateTodoItemHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public Task<Guid> Handle(CreateTodoItemRequest request, CancellationToken cancellationToken)
    {
        var item = new TodoItem
        {
            Created = DateTime.UtcNow,
            Id = Guid.NewGuid(),
            Text = request.Text
        };

        return _todoRepository.Create(item, cancellationToken);
    }
}