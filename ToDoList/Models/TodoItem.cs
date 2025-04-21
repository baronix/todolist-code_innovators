using System;

namespace ToDoList.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public required string Title { get; set; } // Verificador::Acrescentei required
        public bool IsComplete { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
