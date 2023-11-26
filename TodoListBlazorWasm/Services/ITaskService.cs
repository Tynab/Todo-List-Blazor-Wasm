using TodoListBlazorWasm.Models.Dtos;
using TodoListBlazorWasm.Models.Requests.Task;
using TodoListBlazorWasm.Models.Responses;

namespace TodoListBlazorWasm.Services;

public interface ITaskService
{
    public ValueTask<List<TaskResponse>?> GetAll();

    public ValueTask<TaskResponse?> Get(string id);

    public ValueTask<List<TaskResponse>?> Search(TasksSearchDto tasksSearch);

    public ValueTask<bool> Create(TaskCreateRequest request);

    public ValueTask<bool> Edit(string id, TaskEditRequest request);
}
