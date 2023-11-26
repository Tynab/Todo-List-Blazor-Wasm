using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TodoListBlazorWasm.Models.Dtos;
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

    private async Task SearchForm(EditContext context)
    {
        ToastService!.ShowInfo("Seach completed");
        Tasks = await TaskService!.Search(TasksSearch!);
    }

    [Inject]
    private IToastService? ToastService { get; set; }

    [Inject]
    private ITaskService? TaskService { get; set; }

    [Inject]
    private IUserService? UserService { get; set; }

    private List<TaskResponse>? Tasks { get; set; }

    private List<UserResponse>? Assignees { get; set; } = new List<UserResponse>();

    private TasksSearchDto? TasksSearch { get; set; } = new TasksSearchDto();
}
