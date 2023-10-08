using Microsoft.AspNetCore.Identity;
using TodoListBlazorWasm.Api.Entities;
using YANLib;
using static System.DateTime;
using static System.Guid;
using static TodoListBlazorWasm.Api.Enums.Priority;
using static TodoListBlazorWasm.Api.Enums.Status;
using Task = System.Threading.Tasks.Task;

namespace TodoListBlazorWasm.Api.Data;

public class TodoListDbContextSeed
{
    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public async Task SeedAsync(TodoListDbContext context, ILogger<TodoListDbContextSeed> logger)
    {
        if (!context.Users.Any())
        {
            var user = new User
            {
                Id = NewGuid(),
                FirstName = "An",
                LastName = "Yami",
                Email = "yamian@gmail.com",
                PhoneNumber = "0123456789",
                UserName = "yan"
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, "Admin@123");
            _ = await context.Users.AddAsync(user);
            logger.LogInformation("Users add: {User}", user.CamelSerialize());
        }

        if (!context.Tasks.Any())
        {
            var task = new Entities.Task
            {
                Id = NewGuid(),
                Name = "Task 1",
                CreatedDate = UtcNow,
                Priority = High,
                Status = Open
            };

            _ = await context.Tasks.AddAsync(task);
            logger.LogInformation("Tasks add: {Task}", task.CamelSerialize());
        }

        _ = await context.SaveChangesAsync();
    }
}
