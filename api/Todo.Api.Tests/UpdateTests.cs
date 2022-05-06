

using System;
using System.Linq;
using Bogus;
using MediatR;
using Todo.Api.Requests;
using Xunit;

namespace Todo.Api.Tests
{
    public class UpdateTests
    {


        private IMediator Mediator { get; }

        public UpdateTests(IMediator mediator)
        {
            Mediator = mediator;
        }


        [Theory()]
        [InlineData("Start Text", "Update Text")]
        public async void UpdateText(string startText, string updateText)
        {
            var id = await Mediator.Send(new CreateTodoItemRequest() { Text = startText });


            Assert.NotEqual(Guid.Empty, id);


            var items = await Mediator.Send(new ListTodoItemsRequest());
            var item = items.FirstOrDefault(i => i.Id == id);
            Assert.NotNull(item);
            Assert.Equal(startText, item.Text);
            Assert.Null(item.Completed);

            item = await Mediator.Send(new UpdateTodoItemRequest() { Text = updateText, Id = item.Id});
            Assert.NotNull(item);
            Assert.Equal(updateText, item.Text);
            Assert.Null(item.Completed);
        }

        [Fact]
        public async void UpdateCompleted()
        {
            var id = await Mediator.Send(new CreateTodoItemRequest() { Text = "startText" });


            Assert.NotEqual(Guid.Empty, id);


            var items = await Mediator.Send(new ListTodoItemsRequest());
            var item = items.FirstOrDefault(i => i.Id == id);
            Assert.NotNull(item);
            Assert.Null(item.Completed);


            var date = DateTime.Now;
            item = await Mediator.Send(new UpdateTodoItemRequest() { Completed = date, Id = item.Id });
            Assert.NotNull(item);
            Assert.Equal(date, item.Completed);
        }


        [Fact]
        public async void UpdateCompletedDoesntOverrite()
        {
            var id = await Mediator.Send(new CreateTodoItemRequest() { Text = "startText" });


            Assert.NotEqual(Guid.Empty, id);


            var items = await Mediator.Send(new ListTodoItemsRequest());
            var item = items.FirstOrDefault(i => i.Id == id);
            Assert.NotNull(item);
            Assert.Null(item.Completed);


            var date = DateTime.Now;
            item = await Mediator.Send(new UpdateTodoItemRequest() { Completed = date, Id = item.Id });
            Assert.NotNull(item);
            Assert.Equal(date, item.Completed);


            var secondDate = date.AddMonths(-1);
            item = await Mediator.Send(new UpdateTodoItemRequest() { Completed = secondDate, Id = item.Id });
            Assert.NotNull(item);
            Assert.Equal(date, item.Completed);
            Assert.NotEqual(secondDate, item.Completed);

        }
    }
}