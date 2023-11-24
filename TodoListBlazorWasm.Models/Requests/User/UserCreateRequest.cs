namespace TodoListBlazorWasm.Models.Requests.User;

public sealed class UserCreateRequest
{
    public required string UserName { get; set; }

    public required string Password { get; set; }

    public required string Email { get; set; }

    public required string PhoneNumber { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}
