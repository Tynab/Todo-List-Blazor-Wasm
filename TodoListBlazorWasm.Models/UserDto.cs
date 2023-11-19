namespace TodoListBlazorWasm.Models;

public sealed class UserDto
{
    public Guid Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}
