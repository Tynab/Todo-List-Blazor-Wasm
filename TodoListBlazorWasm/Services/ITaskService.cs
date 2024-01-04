using TodoListBlazorWasm.Models.Dtos;
using TodoListBlazorWasm.Models.Requests.Task;
using TodoListBlazorWasm.Models.Responses;
using TodoListBlazorWasm.Models.SeedWork;

namespace TodoListBlazorWasm.Services;

public interface ITaskService
{
    public ValueTask<List<TaskResponse>?> GetAll();

    public ValueTask<TaskResponse?> Get(string? id);

    public ValueTask<bool> Create(TaskCreateRequest? request);

    public ValueTask<bool> Edit(string? id, TaskEditRequest? request);

    public ValueTask<bool> Update(string? id, TaskUpdateRequest? request);

    public ValueTask<bool> Delete(string? id);

    public ValueTask<PagedList<TaskResponse>?> Search(TasksSearchDto? tasksSearch);
}
