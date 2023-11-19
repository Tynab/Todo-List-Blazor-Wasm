using TodoListBlazorWasm.Models.Enums;

namespace TodoListBlazorWasm.Models;

public sealed class TaskInsertRequest
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public Guid? AssigneeId { get; set; }

    public Priority Priority { get; set; }

    public Status Status { get; set; }
}
