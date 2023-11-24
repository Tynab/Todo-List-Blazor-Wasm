namespace TodoListBlazorWasm.Models.Requests.User;

public sealed class UserUpdateRequest
{
    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}
