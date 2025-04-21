using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TodoController : Controller
    {
        private readonly string _dataFilePath = "App_Data/todos.json";

        // Mostra a lista de tarefas
        public IActionResult Index()
        {
            var todos = GetAllTodos();
            return View(todos);
        }

        // Adiciona nova tarefa
        [HttpPost]
        public IActionResult Create(TodoItem todo)
        {
            var todos = GetAllTodos();

            todo.Id = todos.Count > 0 ? todos.Max(t => t.Id) + 1 : 1;
            todo.CreatedAt = DateTime.Now;

            todos.Add(todo);
            SaveTodos(todos);

            return RedirectToAction("Index");
        }

        // Marca/desmarca tarefa como completa
        [HttpPost]
        public IActionResult ToggleComplete(int id)
        {
            var todos = GetAllTodos();
            var todo = todos.Find(t => t.Id == id);

            if (todo != null)
            {
                todo.IsComplete = !todo.IsComplete;
                SaveTodos(todos);
            }

            return RedirectToAction("Index");
        }

        // Remove uma tarefa
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var todos = GetAllTodos();
            var todo = todos.Find(t => t.Id == id);

            if (todo != null)
            {
                todos.Remove(todo);
                SaveTodos(todos);
            }

            return RedirectToAction("Index");
        }

        // Lê todas as tarefas do arquivo JSON
        private List<TodoItem> GetAllTodos()
        {
            EnsureDirectoryExists();

            if (System.IO.File.Exists(_dataFilePath))
            {
                var json = System.IO.File.ReadAllText(_dataFilePath);
                return JsonConvert.DeserializeObject<List<TodoItem>>(json) ?? new List<TodoItem>();
            }

            return new List<TodoItem>();
        }

        // Salva as tarefas no arquivo JSON
        private void SaveTodos(List<TodoItem> todos)
        {
            EnsureDirectoryExists();

            var json = JsonConvert.SerializeObject(todos, Formatting.Indented);
            System.IO.File.WriteAllText(_dataFilePath, json);
        }

        // Garante que o diretório App_Data existe
        private void EnsureDirectoryExists()
        {
            var directory = Path.GetDirectoryName(_dataFilePath);
            if (directory != null && !Directory.Exists(directory)) //Verfificador::confirma se existe
            {
                _ = Directory.CreateDirectory(directory);
            }
            // Alteração 16-04-2025, verificação de ficheiro 
            else if (directory != null)
            {
                // Verifica se o ficheiro existe, senão cria um ficheiro vazio
                if (!System.IO.File.Exists(_dataFilePath))
                {
                    System.IO.File.Create(_dataFilePath).Dispose();
                }
            }
        }
    }
}
