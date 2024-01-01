using TodoListBlazorWasm.Models.Enums;
using TodoListBlazorWasm.Models.SeedWork;

namespace TodoListBlazorWasm.Models.Dtos;

public sealed class TasksSearchDto : PagingParameters
{
    public string? Name { get; set; }

    public Guid? AssigneeId { get; set; }

    public Priority? Priority { get; set; }
}
