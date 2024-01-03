using Microsoft.AspNetCore.Components;
using TodoListBlazorWasm.Components;
using TodoListBlazorWasm.Models.Dtos;
using TodoListBlazorWasm.Models.Responses;
using TodoListBlazorWasm.Models.SeedWork;
using TodoListBlazorWasm.Shared;

namespace TodoListBlazorWasm.Pages;

public sealed partial class TasksPage
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            await GetTasks();
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    public async Task SearchTask(TasksSearchDto tasksSearch)
    {
        try
        {
            TasksSearch = tasksSearch;
            await GetTasks();
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    public void OnDeleteTask(Guid deleteId)
    {
        try
        {
            DeleteId = deleteId;
            DeleteConfirmation?.Show();
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    public void OpenAssignPopup(Guid id)
    {
        try
        {
            AssignTaskDialog?.Show(id);
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    public async Task AssignTaskSuccess(bool result)
    {
        try
        {
            if (result)
            {
                await GetTasks();
            }
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    public async Task OnConfirmDeleteTask(bool deleteConfirmed)
    {
        try
        {
            if (deleteConfirmed && await TaskService.Delete(DeleteId.ToString()))
            {
                await GetTasks();
            }
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
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
            Error?.ProcessError(ex);
        }
    }

    private async Task SelectedPage(int page)
    {
        try
        {
            TasksSearch.PageNumber = page;
            await GetTasks();
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    [CascadingParameter]
    private Error? Error { get; set; }

    private Confirmation? DeleteConfirmation { get; set; }

    private AssignTask? AssignTaskDialog { get; set; }

    private Guid DeleteId { get; set; }

    private List<TaskResponse>? Tasks { get; set; }

    private TasksSearchDto TasksSearch { get; set; } = new TasksSearchDto();

    private MetaData MetaData { get; set; } = new MetaData();
}
