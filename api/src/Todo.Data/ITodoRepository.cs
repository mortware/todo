using Todo.Api.Requests;

namespace Todo.Data;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> List();
    
    Task<Guid> Create(TodoItem newItem);

    Task<TodoItem> Find(Guid id);

    Task Update(TodoItem item);

}