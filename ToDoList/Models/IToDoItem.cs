using System;

namespace ToDoList.Models
{
    public interface IToDoItem
    {
        int Id { get; set; }
        string Title { get; set; }
        bool IsComplete { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
