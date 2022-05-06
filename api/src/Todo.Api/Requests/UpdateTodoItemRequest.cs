namespace Todo.Api.Requests
{
    public class UpdateTodoItemRequest : IRequest<TodoItem>
    {
        public Guid Id { get; set; }

        public string? Text { get; set; }

        public DateTime? Completed { get; set; }

    }
}
