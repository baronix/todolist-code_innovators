using System;

namespace ToDoList.Models
{
    public class TodoItem : IToDoItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
