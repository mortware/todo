namespace Todo.Api.Handlers
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Todo.Api.Requests;

    public class UpdateTodoItemHandler : IRequestHandler<UpdateTodoItemRequest, TodoItem>
    {
        private readonly ITodoRepository _repository;

        public UpdateTodoItemHandler(ITodoRepository repository)
        {
            this._repository = repository;
        }

        public async Task<TodoItem> Handle(UpdateTodoItemRequest request, CancellationToken cancellationToken)
        {
            var existingItem = await this._repository.FindAsync(request.Id, cancellationToken);
            if (existingItem == null)
            {
                throw new HttpRequestException("Item not found", null, HttpStatusCode.NotFound);
            }

            /*  It would be much nicer if we could do this with JsonPatchRequest, but it still depends on Json.NET
                and using it solely for PATCH requests alongside System.Text.Json is a bit awkward. 
                I've opted to set the properties manually here instead */
            if (request.Text != null)
            {
                existingItem.Text = request.Text;
            }

            if (request.Completed.HasValue)
            {
                existingItem.Completed = request.Completed;
            }

            await this._repository.UpdateAsync(existingItem, cancellationToken);
            return await this._repository.FindAsync(existingItem.Id, cancellationToken); ;
        }
    }
}
