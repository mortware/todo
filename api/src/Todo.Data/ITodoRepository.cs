using Todo.Api.Requests;

namespace Todo.Data;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> ListAsync(CancellationToken cancellationToken);
    
    Task<Guid> CreateAsync(TodoItem newItem, CancellationToken cancellationToken);

    Task<int> UpdateAsync(TodoItem item, CancellationToken cancellationToken);

    Task<TodoItem> FindAsync(Guid id, CancellationToken cancellationToken);
}