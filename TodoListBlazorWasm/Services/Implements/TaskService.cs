using System.Net.Http.Json;
using TodoListBlazorWasm.Models.Dtos;
using TodoListBlazorWasm.Models.Requests.Task;
using TodoListBlazorWasm.Models.Responses;
using YANLib;

namespace TodoListBlazorWasm.Services.Implements;

public sealed class TaskService : ITaskService
{
    private readonly HttpClient _httpClient;

    public TaskService(HttpClient httpClient) => _httpClient = httpClient;

    public async ValueTask<List<TaskResponse>?> GetAll() => await _httpClient.GetFromJsonAsync<List<TaskResponse>>("api/tasks");

    public async ValueTask<TaskResponse?> Get(string id) => await _httpClient.GetFromJsonAsync<TaskResponse>($"api/tasks/{id}");

    public async ValueTask<List<TaskResponse>?> Search(TasksSearchDto tasksSearch) => await _httpClient.GetFromJsonAsync<List<TaskResponse>>(
        $"api/tasks/search" +
        $"?{nameof(tasksSearch.Name).ToLowerInvariant()}={tasksSearch.Name}" +
        $"&{nameof(tasksSearch.AssigneeId).ToLowerInvariant()}={tasksSearch.AssigneeId}" +
        $"&{nameof(tasksSearch.Priority).ToLowerInvariant()}={tasksSearch.Priority}"
    );

    public async ValueTask<bool> Create(TaskCreateRequest request)
    {
        var res = await _httpClient.PostAsJsonAsync("api/tasks", request);
        var rslt = (await res.Content.ReadAsStringAsync()).Deserialize<TaskResponse>();

        return res.IsSuccessStatusCode && rslt is not null;
    }

    public async ValueTask<bool> Edit(string id, TaskEditRequest request)
    {
        var res = await _httpClient.PutAsJsonAsync($"api/tasks/{id}", request);
        var rslt = (await res.Content.ReadAsStringAsync()).Deserialize<TaskResponse>();

        return res.IsSuccessStatusCode && rslt is not null;
    }
}
