using NUnit.Framework;
using Todo.Data;
using System;
using Moq;
using Todo.Api.Handlers;
using Todo.Api.Requests;
using System.Threading;

namespace Todo.Api.Tests
{
    public class UpdateTodoItemHandlerTests
    {
        private Mock<ITodoRepository> _mockTodoRepository;
        private UpdateTodoItemHandler handler;
        [SetUp]
        public void Setup()
        {            
            _mockTodoRepository = new Mock<ITodoRepository>();
            handler = new UpdateTodoItemHandler(_mockTodoRepository.Object);
            _mockTodoRepository.Setup(x => x.Update(It.IsAny<TodoItem>())).ReturnsAsync(false);
            _mockTodoRepository.Setup(x => x.GetItem(It.IsAny<Guid>())).ReturnsAsync( new TodoItem
            {                
                Id = Guid.Parse("eef54837-ccff-4764-8c78-286a95776283"),
                Created = DateTime.Parse("2022-05-03T04:21:36.7711066+01:00"),
                Text = "Recusandae accusamus consequatur laudantium eos hic odit minus laudantium odio."
            });
        }

        [Test]
        public void UpdateTodoItemHandler_PassNull_ReturnFalse()
        {
            var request = new UpdateTodoItemRequest();
            var output = handler.Handle(request, CancellationToken.None);            
            Assert.IsFalse(output.Result);
        }
        
        [Test]
        public void UpdateTodoItemHandler_PassNull_ReturnNotFound()
        {
            var request = new UpdateTodoItemRequest
            {
                Id = Guid.NewGuid()
            };
            var output = handler.Handle(request, CancellationToken.None);
            Assert.AreEqual(output.Result, false);           
        }

        [Test]
        public void UpdateTodoItemHandler_PassNull_ReturnFalseFromUpdate()
        {
            _mockTodoRepository.Setup(x => x.Update(It.IsAny<TodoItem>())).ReturnsAsync(false);
            var request = new UpdateTodoItemRequest
            {
                Id = Guid.Parse("eef54837-ccff-4764-8c78-286a95776283")
            };
            var output = handler.Handle(request, CancellationToken.None);
            Assert.AreEqual(output.Result, false);

        }

        [Test]
        public void UpdateTodoItemHandler_PassNull_ReturnTrue()
        {
            _mockTodoRepository.Setup(x => x.Update(It.IsAny<TodoItem>())).ReturnsAsync(true);
            var request = new UpdateTodoItemRequest
            {
                Id = Guid.Parse("eef54837-ccff-4764-8c78-286a95776283")
            };
            var output = handler.Handle(request, CancellationToken.None);
            Assert.IsTrue(output.Result);           

        }
    }
}