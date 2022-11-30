using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.DTOS;

public struct CreateTodoDTO
{
    [Required(ErrorMessage = "Please Enter valid content"), DataType(DataType.Text), MinLength(4)]
    public string Content { get; set; }
    [Required]
    public int ListId { get; set; }
}