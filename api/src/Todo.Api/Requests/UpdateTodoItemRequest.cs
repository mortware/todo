namespace Todo.Api.Requests;

public class UpdateTodoItemRequest : IRequest<bool>
{
    public Guid Id { get; set; }
}