using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TodoController : Controller
    {
        #region Private Atribute
        private readonly string _dataFilePath;
        #endregion Private Atribute

        #region Private Method
        // Garante que o diretório App_Data existe
        private void EnsureDirectoryExists()
        {
            var directory = Path.GetDirectoryName(_dataFilePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            else if (directory != null)
            {
                if (!System.IO.File.Exists(_dataFilePath))
                {
                    System.IO.File.Create(_dataFilePath).Dispose();
                }
            }
        }
        #endregion Private Method

        #region Public Methods
        public TodoController(string dataFilePath = "App_Data/todos.json")
        {
            _dataFilePath = dataFilePath;
        }

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
            if (string.IsNullOrEmpty(todo.Title))
            {
                ModelState.AddModelError("Title", "The Title field is required.");
                return View(todo);
            }

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

            if (todo == null)
            {
                return NotFound();
            }

            todos.Remove(todo);
            SaveTodos(todos);

            return RedirectToAction("Index");
        }

        // Lê todas as tarefas do arquivo JSON
        public List<TodoItem> GetAllTodos()
        {
           // Garante que o diretório App_Data existe
            EnsureDirectoryExists();
             // Valida e verifica se o ficheiro existe 
            if (System.IO.File.Exists(_dataFilePath))
            {
                var json = System.IO.File.ReadAllText(_dataFilePath);
                return JsonConvert.DeserializeObject<List<TodoItem>>(json) ?? new List<TodoItem>();
            }

            return new List<TodoItem>();
        }

        // Salva as tarefas no arquivo JSON
        public void SaveTodos(List<TodoItem> todos)
        {
            EnsureDirectoryExists();

            var json = JsonConvert.SerializeObject(todos, Formatting.Indented);
            System.IO.File.WriteAllText(_dataFilePath, json);
        }
        #endregion Public Methods
    }
}
