using Todo.Api.Requests;

namespace Todo.Data;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> List();
    
    Task<Guid> Create(TodoItem newItem);

    Task<TodoItem> GetItem(Guid id);

    Task<bool> Update(TodoItem updatedItem);
}