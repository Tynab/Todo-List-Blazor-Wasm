using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoListBlazorWasm.Api.Entities;

namespace TodoListBlazorWasm.Api.Data;

public class TodoListDbContext : IdentityDbContext<User, Role, Guid>
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options)
    {
    }

    public DbSet<Entities.Task> Tasks { get; set; }
}
