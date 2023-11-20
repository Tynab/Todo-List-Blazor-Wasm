using Microsoft.AspNetCore.Components;
using TodoListBlazorWasm.Models.Responses;
using TodoListBlazorWasm.Services;

namespace TodoListBlazorWasm.Pages;

public partial class TaskList
{
    private List<TaskResponse>? _tasks;

    protected override async Task OnInitializedAsync() => _tasks = await TaskService!.GetAll();

    [Inject] private ITaskService? TaskService { get; set; }
}
