namespace TodoListBlazorWasm.Models.Responses;

public sealed class RoleResponse
{
    public Guid Id { get; set; }

    public required string Description { get; set; }
}
