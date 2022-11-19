namespace TodoApp.Models.Entity;

public class Todo : BaseEntity
{
    public int Id { get; set; }
    public string Content { get; set; }
}