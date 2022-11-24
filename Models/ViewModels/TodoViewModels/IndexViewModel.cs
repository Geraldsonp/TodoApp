using TodoApp.Models.Entity;

namespace TodoApp.Models.ViewModels.TodoViewModels;

public struct IndexViewModel
{
    public IEnumerable<Todo>? Todos { get; set; }

    public string Content { get; set; }
}