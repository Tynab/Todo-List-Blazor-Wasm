using Microsoft.AspNetCore.Components;
using TodoListBlazorWasm.Components;
using TodoListBlazorWasm.Models.Dtos;
using TodoListBlazorWasm.Models.Responses;
using TodoListBlazorWasm.Models.SeedWork;
using TodoListBlazorWasm.Shared;

namespace TodoListBlazorWasm.Pages;

public sealed partial class TasksPage
{
    protected override async Task OnInitializedAsync() => await GetTasks();

    public async Task SearchTask(TasksSearchDto tasksSearch)
    {
        TasksSearch = tasksSearch;
        await GetTasks();
    }

    public void OnDeleteTask(Guid deleteId)
    {
        DeleteId = deleteId;
        DeleteConfirmation?.Show();
    }

    public void OpenAssignPopup(Guid id) => AssignTaskDialog?.Show(id);

    public async Task AssignTaskSuccess(bool result)
    {
        if (result)
        {
            await GetTasks();
        }
    }

    public async Task OnConfirmDeleteTask(bool deleteConfirmed)
    {
        if (deleteConfirmed && await TaskService.Delete(DeleteId.ToString()))
        {
            await GetTasks();
        }
    }

    private async Task GetTasks()
    {
        try
        {
            var pagingRes = await TaskService.Search(TasksSearch);

            if (pagingRes is not null && pagingRes.MetaData is not null)
            {
                Tasks = pagingRes.Items;
                MetaData = pagingRes.MetaData;
            }
        }
        catch (Exception ex)
        {
            Error.ProcessError(ex);
        }
    }

    private async Task SelectedPage(int page)
    {
        TasksSearch.PageNumber = page;
        await GetTasks();
    }

    [CascadingParameter]
    private Error Error { get; set; }

    private Confirmation? DeleteConfirmation { get; set; }

    private AssignTask? AssignTaskDialog { get; set; }

    private Guid DeleteId { get; set; }

    private List<TaskResponse>? Tasks { get; set; }

    private TasksSearchDto TasksSearch { get; set; } = new TasksSearchDto();

    private MetaData MetaData { get; set; } = new MetaData();
}
