using Todo.Api.Requests;

namespace Todo.Data;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> List(CancellationToken cancellationToken);
    
    Task<Guid> Create(TodoItem newItem, CancellationToken cancellationToken);
}