using TodoListBlazorWasm.Models.Enums;

namespace TodoListBlazorWasm.Models.Dtos;

public sealed class TasksSearchDto
{
    public string? Name { get; set; }

    public Guid? AssigneeId { get; set; }

    public Priority? Priority { get; set; }
}
