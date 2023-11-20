using TodoListBlazorWasm.Models.Responses;

namespace TodoListBlazorWasm.Services;

public interface ITaskService
{
    public ValueTask<List<TaskResponse>?> GetAll();

    public ValueTask<TaskResponse?> Get(string id);
}
