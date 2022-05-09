namespace Todo.Api.UnitTests.Controllers
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Todo.Api.Controllers;
    using Todo.Api.Requests;
    using Xunit;

    public class TodoControllerTests
    {
        [Fact]
        public void Controller_Type_ImplementsExpectedAttributesAndDerivedTypes()
        {
            // Arrange
            var type = typeof(TodoController);

            // Assert
            var attributes = type.GetCustomAttributes(false);
            attributes.Should().HaveCount(1);
            attributes.Should().ContainSingle(x => x is ApiControllerAttribute);
            type.BaseType.Should().Be(typeof(ControllerBase));
        }

        [Fact]
        public async Task Update_WithModelAndCancellationToken_ReturnsOkObjectResultAndCallsMediatorWithExpectedArguments()
        {
            // Arrange
            var request = new UpdateTodoItemRequest
            {
                Text = "write unit tests for TodoController.Create",
                Completed = new DateTime(2022, 05, 07),
            };
            var serviceResult = new TodoItem
            {
                Created = new DateTime(2020, 01, 01),
                Text = "go outside",
            };
            var mediator = new Mock<IMediator>(MockBehavior.Strict);
            mediator.Setup(x => x.Send(It.IsAny<UpdateTodoItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(serviceResult);
            var sut = new TodoController(mediator.Object);

            // Act
            var result = await sut.Update(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeOfType<TodoItem>();
            var responseModel = okResult.Value.As<TodoItem>();
            responseModel.Should().Be(serviceResult);
            responseModel.Created.Should().Be(new DateTime(2020, 01, 01));
            responseModel.Text.Should().Be("go outside");
            responseModel.Id.Should().Be(default(Guid));
            responseModel.Completed.Should().Be(default(DateTime?));
            mediator.Verify(x => x.Send(request, CancellationToken.None), Times.Once());
            mediator.VerifyNoOtherCalls();
        }
    }
}
