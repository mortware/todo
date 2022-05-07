namespace Todo.Api.UnitTests.Handlers
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Moq;
    using Todo.Api.Handlers;
    using Todo.Api.Requests;
    using Todo.Data;
    using Xunit;

    public class UpdateTodoItemHandlerTests
    {
        #region Test cases
        public static readonly object[][] TestCases = new object[][]
        {
            new object[]
            {
                new UpdateTodoItemRequest
                {
                    Id = Guid.Parse("13371337-dead-beef-7ac0-1ee71ee71ee7"),
                    Text = "Teach tortoise how to fly",
                    Completed = new DateTime(2020, 01, 01),
                    Created = new DateTime(1920, 01, 01),
                },
                new TodoItem
                {
                    Completed = new DateTime(2020, 12, 12),
                    Text = "overwrite me pls",
                    Id = Guid.Empty,
                    Created = new DateTime(1920, 02, 02),
                },
                "Teach tortoise how to fly",
                new DateTime(2020, 01, 01),
            },
            new object[]
            {
                new UpdateTodoItemRequest
                {
                    Id = Guid.Parse("13371337-dead-beef-7ac0-1ee71ee71ee7"),
                    Text = default(string),
                    Completed = default(DateTime?),
                },
                new TodoItem
                {
                    Completed = new DateTime(2020, 12, 12),
                    Text = "don't overwrite me pls",
                    Id = Guid.Empty,
                    Created = new DateTime(1920, 02, 02),
                },
                "don't overwrite me pls",
                new DateTime(2020, 12, 12),
            },
        };
        #endregion

        private readonly UpdateTodoItemHandler sut;
        private readonly Mock<ITodoRepository> repository;

        public UpdateTodoItemHandlerTests()
        {
            this.repository = new Mock<ITodoRepository>(MockBehavior.Strict);
            this.sut = new UpdateTodoItemHandler(this.repository.Object);
        }

        [Theory]
        [MemberData(nameof(UpdateTodoItemHandlerTests.TestCases))]
        public async Task Handle_ValidRequestWithUpdatedProperties_MakesExpectedCallsAndSetsExpectedProperties(UpdateTodoItemRequest request, TodoItem existingItem, string expectedTextValue, DateTime? expectedCompletedValue)
        {
            // Arrange
            var returnedItem = new TodoItem
            {
                Text = "final result",
                Completed = DateTime.MaxValue,
                Created = DateTime.MinValue,
                Id = Guid.Parse("01234567-890a-bcde-fedc-a09876543210")
            };
            this.repository.SetupSequence(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingItem)
                .ReturnsAsync(returnedItem);
            this.repository.Setup(x => x.UpdateAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            var result = await this.sut.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(returnedItem);
            result.Text.Should().Be("final result");
            result.Completed.Should().Be(DateTime.MaxValue);
            result.Created.Should().Be(DateTime.MinValue);
            result.Id.Should().Be(Guid.Parse("01234567-890a-bcde-fedc-a09876543210"));
            this.repository.Verify(x => x.FindAsync(request.Id, CancellationToken.None), Times.Once);
            this.repository.Verify(x => x.FindAsync(existingItem.Id, CancellationToken.None), Times.Once);
            this.repository.Verify(
                x => x.UpdateAsync(
                    It.Is<TodoItem>(
                        y => y.Text == expectedTextValue &&
                            y.Completed == expectedCompletedValue &&
                            y.Created == existingItem.Created &&
                            y.Id == existingItem.Id),
                    CancellationToken.None),
                Times.Once());
            this.repository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_FindAsyncReturnsNull_ThrowsHttpRequestException()
        {
            // Arrange
            this.repository.Setup(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(TodoItem));
            var guid = Guid.Parse("13371337-dead-beef-7ac0-1ee71ee71ee7");

            // Act
            var resultFunc = () => this.sut.Handle(new UpdateTodoItemRequest { Id = guid }, CancellationToken.None);

            // Assert
            (await resultFunc.Should().ThrowExactlyAsync<HttpRequestException>())
                .Where(x => x.StatusCode == System.Net.HttpStatusCode.NotFound && x.InnerException == null)
                .WithMessage("Item not found");
            this.repository.Verify(x => x.FindAsync(guid, CancellationToken.None), Times.Once);
            this.repository.VerifyNoOtherCalls();
        }
    }
}
