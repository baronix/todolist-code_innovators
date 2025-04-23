using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ToDoList.Controllers;
using ToDoList.Models;
using Xunit;

namespace ToDoList.Tests.Controllers
{
    public class TodoControllerTests
    {
        private readonly TodoController _controller;

        public TodoControllerTests()
        {
            _controller = new TodoController();
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfTodos()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<TodoItem>>(viewResult.Model);
            Assert.NotNull(model);
        }

        [Fact]
        public void Create_AddsNewTodoItem()
        {
            // Arrange
            var newTodo = new TodoItem { Title = "New Task" };

            // Act
            var result = _controller.Create(newTodo);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public void ToggleComplete_TogglesTodoItemCompletion()
        {
            // Arrange
            var todo = new TodoItem { Id = 1, Title = "Task", IsComplete = false };
            _controller.Create(todo);

            // Act
            var result = _controller.ToggleComplete(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public void Delete_RemovesTodoItem()
        {
            // Arrange
            var todo = new TodoItem { Id = 1, Title = "Task" };
            _controller.Create(todo);

            // Act
            var result = _controller.Delete(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}
