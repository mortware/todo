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

    public async Task<IEnumerable<TodoItem>> List()
    {
        return await _context
            .TodoItems
            .ToArrayAsync();
    }

    public async Task<Guid> Create(TodoItem newItem)
    {
        await _context.TodoItems.AddAsync(newItem);
        await _context.SaveChangesAsync();
        return newItem.Id;
    }

    public async Task<TodoItem> Find(Guid id)
    {
        return await _context.TodoItems.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task Update(TodoItem item)
    {
        _context.TodoItems.Update(item);
        await _context.SaveChangesAsync();
    }
}