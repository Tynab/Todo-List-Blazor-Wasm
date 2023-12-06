using Microsoft.AspNetCore.Components.Forms;
using TodoListBlazorWasm.Models.Dtos;
using TodoListBlazorWasm.Models.Responses;
using static System.Threading.Tasks.Task;

namespace TodoListBlazorWasm.Pages;

public sealed partial class TasksPage
{
    protected override async Task OnInitializedAsync()
    {
        var tasksTask = TaskService.GetAll().AsTask();
        var assigneesTask = UserService.GetAll().AsTask();

        await WhenAll(tasksTask, assigneesTask);
        Tasks = await tasksTask;
        Assignees = await assigneesTask;
    }

    private async Task SearchForm(EditContext context)
    {
        Tasks = await TaskService.Search(TasksSearch);
        ToastService.ShowInfo("Seach completed");
    }

    private List<TaskResponse>? Tasks { get; set; }

    private List<UserResponse>? Assignees { get; set; } = new List<UserResponse>();

    private TasksSearchDto? TasksSearch { get; set; } = new TasksSearchDto();
}
