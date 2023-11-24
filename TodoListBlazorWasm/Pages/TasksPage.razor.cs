using Microsoft.AspNetCore.Components;
using TodoListBlazorWasm.Models.Enums;
using TodoListBlazorWasm.Models.Responses;
using TodoListBlazorWasm.Services;

namespace TodoListBlazorWasm.Pages;

public sealed partial class TasksPage
{
    protected override async Task OnInitializedAsync()
    {
        Tasks = await TaskService!.GetAll();
        Assignees = await UserService!.GetAll();
    }

    [Inject]
    private ITaskService? TaskService { get; set; }

    [Inject]
    private IUserService? UserService { get; set; }

    private List<TaskResponse>? Tasks { get; set; }

    private List<UserResponse>? Assignees { get; set; } = new List<UserResponse>();

    private TasksSearch? TasksSearch { get; set; } = new TasksSearch();
}

public sealed class TasksSearch
{
    public string? Name { get; set; }

    public Guid AssigneeId { get; set; }

    public Priority Priority { get; set; }
}