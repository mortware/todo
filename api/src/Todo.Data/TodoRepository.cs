using Microsoft.EntityFrameworkCore;
using Todo.Api.Requests;

namespace Todo.Data;

public class TodoRepository: ITodoRepository
{
    private readonly TodoContext _context;

    public TodoRepository(TodoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context
            .TodoItems
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Guid> CreateAsync(TodoItem newItem, CancellationToken cancellationToken)
    {
        await _context.TodoItems.AddAsync(newItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return newItem.Id;
    }

    public Task<int> UpdateAsync(TodoItem item, CancellationToken cancellationToken)
    {
        this._context.TodoItems.Update(item);
        return this._context.SaveChangesAsync(cancellationToken);
    }

    public Task<TodoItem> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        return this._context.TodoItems.FindAsync(id).AsTask();
    }
}