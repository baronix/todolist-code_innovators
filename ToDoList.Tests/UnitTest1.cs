using Microsoft.AspNetCore.Mvc;
using ToDoList.Controllers;
using ToDoList.Models;


namespace ToDoList.Tests
{
    public class UnitTest1
    {
        #region Private Attribute
        private readonly TodoController _controller;
        #endregion Private Attribute

        #region Private Methods
        private string GetTempFilePath()
        {
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".json");
        }
        #endregion Private Methods

        #region Public Methods
        public UnitTest1()
        {
            _controller = new TodoController();
        }

        // Teste para verificar se o método Index retorna uma lista de todos os itens
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

        // Teste para verificar se o método Create adiciona um novo item
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

        // Teste para verificar se o método ToggleComplete alterna o estado de conclusão de um item
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

        // Teste para verificar se o método Delete remove um item

        [Fact]
        public void Delete_RemovesTodoItem()
        {
            // Arrange
            var controller = new TodoController();
            var newTodo = new TodoItem { Title = "Dummy" };

            // Act - Criar a nova tarefa
            var createResult = controller.Create(newTodo);
            var redirectResult = Assert.IsType<RedirectToActionResult>(createResult);
            Assert.Equal("Index", redirectResult.ActionName);

            // Verificar se a tarefa foi criada
            var todos = controller.GetAllTodos();
            var createdTodo = Assert.Single(todos, t => t.Title == "Dummy");

            // Act - Apagar a tarefa criada
            var deleteResult = controller.Delete(createdTodo.Id);
            var deleteRedirectResult = Assert.IsType<RedirectToActionResult>(deleteResult);
            Assert.Equal("Index", deleteRedirectResult.ActionName);

            // Verificar se a tarefa foi removida
            todos = controller.GetAllTodos();
            Assert.DoesNotContain(todos, t => t.Id == createdTodo.Id);

        }

        // Teste para verificar se a exclusão de um item inexistente retorna NotFound
        [Fact]
        public void Delete_NonExistentItem_ReturnsNotFound()
        {
            // Arrange
            var nonExistentId = 9999;
            var controller = new TodoController();

            // Act
            var result = controller.Delete(nonExistentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public void CreateLargeNumberOfTodoItems()
        {
            // Arrange
            var tempFilePath = GetTempFilePath();
            var controller = new TodoController(tempFilePath);
            var todos = new List<TodoItem>();

            for (int i = 0; i < 1000; i++)
            {
                todos.Add(new TodoItem { Title = $"Task {i + 1}" });
            }

            // Act - Criar todas as tarefas
            foreach (var todo in todos)
            {
                var createResult = controller.Create(todo);
                var redirectResult = Assert.IsType<RedirectToActionResult>(createResult);
                Assert.Equal("Index", redirectResult.ActionName);
            }

            // Verificar se todas as tarefas foram criadas
            var createdTodos = controller.GetAllTodos();
            Assert.Equal(1000, createdTodos.Count);
            for (int i = 0; i < 1000; i++)
            {
                Assert.Contains(createdTodos, t => t.Title == $"Task {i + 1}");
            }

            // Cleanup
            File.Delete(tempFilePath);
        }
        #endregion Public Methods
    }
}
