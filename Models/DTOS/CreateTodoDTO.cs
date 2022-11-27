namespace TodoApp.Models.DTOS;

public struct CreateTodoDTO
{
    public string Content { get; set; }
    public int ListId { get; set; }
}