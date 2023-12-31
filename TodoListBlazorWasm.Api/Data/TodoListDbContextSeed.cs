﻿using Microsoft.AspNetCore.Identity;
using TodoListBlazorWasm.Api.Entities;
using YANLib;
using static System.DateTime;
using static System.Guid;
using static TodoListBlazorWasm.Models.Enums.Priority;
using static TodoListBlazorWasm.Models.Enums.Status;
using Task = System.Threading.Tasks.Task;

namespace TodoListBlazorWasm.Api.Data;

public sealed class TodoListDbContextSeed
{
    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public async Task SeedAsync(ILogger<TodoListDbContextSeed>? logger, TodoListDbContext context)
    {
        if (!context.Users.Any())
        {
            var user = new User
            {
                Id = NewGuid(),
                FirstName = "An",
                LastName = "Yami",
                Email = "yamian@gmail.com",
                NormalizedEmail = "YAMIAN@GMAIL.COM",
                PhoneNumber = "0123456789",
                UserName = "yan",
                NormalizedUserName = "YAN",
                SecurityStamp = NewGuid().ToString()
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, "Admin@123");
            _ = await context.Users.AddAsync(user);
            logger?.LogInformation("Users add: {User}", user.Serialize());
        }

        if (!context.Tasks.Any())
        {
            var task = new Entities.Task
            {
                Id = NewGuid(),
                Name = "Task 1",
                CreatedAt = UtcNow,
                Priority = High,
                Status = Open
            };

            _ = await context.Tasks.AddAsync(task);
            logger?.LogInformation("Tasks add: {Task}", task.Serialize());
        }

        _ = await context.SaveChangesAsync();
    }
}
