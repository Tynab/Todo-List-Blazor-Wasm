using System.ComponentModel.DataAnnotations;

namespace TodoListBlazorWasm.Models.Requests.User;

public sealed class UserEditRequest
{
    [Required]
    public string? Password { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? PhoneNumber { get; set; }

    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }
}
