using Microsoft.AspNetCore.Components;
using TodoListBlazorWasm.Models.Responses;
using TodoListBlazorWasm.Services;

namespace TodoListBlazorWasm.Pages;

public sealed partial class TaskListPage
{

    protected override async Task OnInitializedAsync() => Tasks = await TaskService!.GetAll();

    [Inject]
    private ITaskService? TaskService { get; set; }

    private List<TaskResponse>? Tasks { get; set; }
}
