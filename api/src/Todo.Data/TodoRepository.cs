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

    public async Task<IEnumerable<TodoItem>> List(CancellationToken cancellationToken)
    {
        return await _context
            .TodoItems
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Guid> Create(TodoItem newItem, CancellationToken cancellationToken)
    {
        await _context.TodoItems.AddAsync(newItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return newItem.Id;
    }
}