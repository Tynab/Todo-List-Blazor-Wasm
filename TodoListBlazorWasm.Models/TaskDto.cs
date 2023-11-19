using TodoListBlazorWasm.Models.Enums;

namespace TodoListBlazorWasm.Models;

public sealed class TaskDto
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public Priority Priority { get; set; }

    public Status Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public UserDto? Assignee { get; set; }
}
