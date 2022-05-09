using Todo.Api.Requests;

namespace Todo.Api.Handlers;

public class UpdateTodoItemHandler : IRequestHandler<UpdateTodoItemRequest, bool>
{
    private readonly ITodoRepository _todoRepository;

    public UpdateTodoItemHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }    

    public async Task<bool> Handle(UpdateTodoItemRequest request, CancellationToken cancellationToken)
    {
        var item = await _todoRepository.GetItem(request.Id);
        if (item == null || item.Completed != null) return false;
        item.Completed = DateTime.Now;
        return await _todoRepository.Update(item);
    }
}