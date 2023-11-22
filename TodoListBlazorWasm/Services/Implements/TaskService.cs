using System.Net.Http.Json;
using TodoListBlazorWasm.Models.Responses;

namespace TodoListBlazorWasm.Services.Implements;

public class TaskService : ITaskService
{
    private readonly HttpClient _httpClient;

    public TaskService(HttpClient httpClient) => _httpClient = httpClient;

    public async ValueTask<List<TaskResponse>?> GetAll() => await _httpClient.GetFromJsonAsync<List<TaskResponse>>("/api/tasks");

    public async ValueTask<TaskResponse?> Get(string id) => await _httpClient.GetFromJsonAsync<TaskResponse>($"/api/tasks/{id}");
}
