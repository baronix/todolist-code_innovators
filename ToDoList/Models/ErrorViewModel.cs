namespace ToDoList.Models
{
    // Classe Responsavel pela verificação e validação da tarefa inserida
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
