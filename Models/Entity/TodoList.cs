using System.Text.Json.Serialization;

namespace TodoApp.Models.Entity;

public class TodoList : BaseEntity
{
    public string ListName { get; set; }
    public virtual ICollection<Todo> Todos { get; set; }


    [JsonIgnore]
    public string OwnerId { get; set; }

    [JsonIgnore]
    public virtual User Owner { get; set; }

}