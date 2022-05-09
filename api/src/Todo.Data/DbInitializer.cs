using Bogus;
using Todo.Api.Requests;

namespace Todo.Data;

public class DbInitializer : IDisposable, IAsyncDisposable
{
    private readonly TodoContext _dbContext;

    public DbInitializer(TodoContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public void Dispose()
    {
        if (_dbContext != null)
        {
            _dbContext.Dispose();
        }
    }

    public ValueTask DisposeAsync()
    {
        if (_dbContext != null)
        {
            return _dbContext.DisposeAsync();
        }

        return ValueTask.CompletedTask;
    }

    public void Seed()
    {
        Randomizer.Seed = new Random(1337);
        var itemFaker = new Faker<TodoItem>()
            .RuleFor(t => t.Id, f => Guid.NewGuid())
            .RuleFor(t => t.Completed, f => f.Random.Bool() ? f.Date.Past() : null)
            .RuleFor(t => t.Created, (f, t) => f.Date.Past(refDate: t.Completed))
            .RuleFor(t => t.Text, f => f.Lorem.Sentence());

        var items = itemFaker.Generate(5);

        _dbContext.RemoveRange(this._dbContext.TodoItems);
        _dbContext.TodoItems.AddRange(items);
        _dbContext.SaveChanges();
    }
}