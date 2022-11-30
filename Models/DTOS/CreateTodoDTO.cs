using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.DTOS;

public struct CreateTodoDTO
{
    [Required(ErrorMessage = "Please Enter a valid content")]
    public string Content { get; set; }
    [Required]
    public int ListId { get; set; }
}