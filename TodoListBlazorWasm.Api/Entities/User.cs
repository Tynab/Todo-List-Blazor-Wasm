using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TodoListBlazorWasm.Api.Entities;

public sealed class User : IdentityUser<Guid>
{
    [MaxLength(100)]
    public required string FirstName { get; set; }

    [MaxLength(100)]
    public required string LastName { get; set; }

    public ICollection<Task>? Tasks { get; set; }
}
