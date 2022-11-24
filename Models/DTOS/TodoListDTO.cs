using TodoApp.Models.Entity;

namespace TodoApp.Models.DTOS;

public struct TodoListDto
{

    public string ListName { get; set; }
    public IEnumerable<Todo> Todos { get; set; }
    public int Id { get; set; }

}