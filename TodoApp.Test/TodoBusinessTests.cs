using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Moq;
using TodoApp.Models.DTOS;
using TodoApp.Models.Entity;
using TodoApp.Services;
using TodoApp.Services.Interfaces;
using Xunit;
using FluentAssertions;
using TodoApp.Models;

namespace TodoApp.Test;

public class TodoBusinessTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
    private readonly Mock<IUserIdProvider> _userIdProviderMock = new Mock<IUserIdProvider>();
    private readonly Fixture _fixture;

    public TodoBusinessTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void WhenValidDataIsProvided_NewTodoIsCreated()
    {
        //Arrange
        var createTodo = new CreateTodoDTO()
        {
            Content = "This is an amazing content",
            ListId = 1
        };

        Todo createdTodo = new Todo();

        _unitOfWorkMock.Setup(x => x.TodoListRepo.DoesExist(It.IsAny<int>())).Returns(true);
        _unitOfWorkMock.Setup(x => x.TodoListRepo.Get(It.IsAny<int>())).Returns(new TodoList(){ Todos = new List<Todo>()});

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.TodoRepo.Create(It.IsAny<Todo>())).Callback<Todo>((entity) => createdTodo = entity);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.SaveChanges());
        _userIdProviderMock.Setup(provider => provider.GetCurrentUserId()).Returns(It.IsAny<string>());

        var todoBusiness = new TodoBusiness(_unitOfWorkMock.Object, _userIdProviderMock.Object);

        //Act
        var result = todoBusiness.Add(createTodo);

        //Assert
        _unitOfWorkMock.Verify(x => x.TodoRepo.Create(createdTodo));
        _unitOfWorkMock.Verify(x => x.SaveChanges());
        result.Data.Should().BeOfType(typeof(Todo));
        result.Data.Content.Should().Be(createTodo.Content);
        result.Data.TodoListId.Should().Be(createTodo.ListId);
    }

    [Fact]
    public void WhenListDoesNotExist_NotFoundErrorIsReturned()
    {
        //Arrange
        _unitOfWorkMock.Setup(x => x.TodoListRepo.DoesExist(It.IsAny<int>())).Returns(false);

        var todoBusiness = new TodoBusiness(_unitOfWorkMock.Object, _userIdProviderMock.Object);

        //Act
        var result = todoBusiness.Add(It.IsAny<CreateTodoDTO>());

        //Assert
        result.Data.Should().BeNull();
        result.ErrorType.Should().Be(ErrorTypes.EntityNotFound);
    }

    [Fact]
    public void WhenListHasMoreThan12_NotSpaceErrorIsReturned()
    {
        //Arrange
        var list = _fixture.Build<TodoList>().With(x => x.Todos, _fixture.CreateMany<Todo>(12).ToList()).Create();

        _unitOfWorkMock.Setup(x => x.TodoListRepo.DoesExist(It.IsAny<int>())).Returns(true);
        _unitOfWorkMock.Setup(x => x.TodoListRepo.Get(It.IsAny<int>())).Returns(list);

        var todoBusiness = new TodoBusiness(_unitOfWorkMock.Object, _userIdProviderMock.Object);

        //Act
        var result = todoBusiness.Add(It.IsAny<CreateTodoDTO>());

        //Assert
        result.Data.Should().BeNull();
        result.ErrorType.Should().Be(ErrorTypes.ListDoesNotHasSpace);
    }

    [Fact]
    public void WhenValidDataIsProvided_TodoIsRemovedFromDb()
    {
        //Arrange
        _unitOfWorkMock.Setup(x => x.TodoRepo.DoesExist(It.IsAny<int>())).Returns(true);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.TodoRepo.Get(It.IsAny<int>())).Returns(new Todo());
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.TodoRepo.Delete(It.IsAny<Todo>()));
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.SaveChanges());

        var todoBusiness = new TodoBusiness(_unitOfWorkMock.Object, _userIdProviderMock.Object);

        //Act
        var result = todoBusiness.Remove(It.IsAny<int>());

        //Assert
        _unitOfWorkMock.Verify(x => x.TodoRepo.DoesExist(It.IsAny<int>()));
        _unitOfWorkMock.Verify(x => x.TodoRepo.Delete(It.IsAny<Todo>()));
        _unitOfWorkMock.Verify(x => x.TodoRepo.Get(It.IsAny<int>()));
        _unitOfWorkMock.Verify(x => x.SaveChanges());
        result.Succeded.Should().BeTrue();
    }

    [Fact]
    public void WhenInValidIdIsProvided_ErrorIsReturned()
    {
        //Arrange
        _unitOfWorkMock.Setup(x => x.TodoRepo.DoesExist(It.IsAny<int>())).Returns(false);

        var todoBusiness = new TodoBusiness(_unitOfWorkMock.Object, _userIdProviderMock.Object);

        //Act
        var result = todoBusiness.Remove(1);

        //Assert
        _unitOfWorkMock.Verify(x => x.TodoRepo.DoesExist(It.IsAny<int>()));
        result.Succeded.Should().BeFalse();
    }

}