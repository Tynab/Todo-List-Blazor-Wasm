using System.ComponentModel.DataAnnotations;
using TodoListBlazorWasm.Models.Enums;

namespace TodoListBlazorWasm.Models.Requests.Task;

public sealed class TaskEditRequest
{
    [Required(ErrorMessage = "Please enter your task name")]
    public string? Name { get; set; }

    [Required]
    public Guid AssigneeId { get; set; }

    [Required]
    public Priority Priority { get; set; }

    [Required]
    public Status Status { get; set; }
}
