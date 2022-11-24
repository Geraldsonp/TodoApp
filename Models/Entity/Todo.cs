using System.Text.Json.Serialization;

namespace TodoApp.Models.Entity;

public class Todo : BaseEntity
{
    public int Id { get; set; }
    public string Content { get; set; }
    public bool IsCompleted { get; set; }


    [JsonIgnore]
    public string OwnerId { get; set; }
    [JsonIgnore]
    public virtual TodoList TodoList { get; set; }
    [JsonIgnore]
    public int TodoListId { get; set; }
}