using Todo.Api.Requests;

namespace Todo.Api.Handlers
{
    public class UpdateTodoItemHandler : IRequestHandler<UpdateTodoItemRequest, TodoItem>
    {


        private readonly ITodoRepository _todoRepository;

        public UpdateTodoItemHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }



        public async Task<TodoItem> Handle(UpdateTodoItemRequest request, CancellationToken cancellationToken)
        {
            var item = await _todoRepository.Find(request.Id);
            if (item == null)
                return null;

            item.Completed = item.Completed ?? request.Completed;
            item.Text = request.Text?? request.Text;

            await _todoRepository.Update(item);
            return item;
        }
    }

}
