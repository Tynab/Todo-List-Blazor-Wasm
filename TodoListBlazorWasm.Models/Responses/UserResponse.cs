namespace TodoListBlazorWasm.Models.Responses;

public sealed class UserResponse
{
    public Guid Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}
