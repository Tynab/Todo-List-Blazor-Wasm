using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TodoListBlazorWasm.Api.Entities;

public class Role : IdentityRole<Guid>
{
    [MaxLength(250)]
    public required string Description { get; set; }
}
