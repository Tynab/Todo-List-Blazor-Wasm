using System.Net.Http.Json;
using TodoListBlazorWasm.Models.Responses;

namespace TodoListBlazorWasm.Services.Implements;

public class TaskService : ITaskService
{
    private readonly HttpClient _httpClient;

    public TaskService(HttpClient httpClient) => _httpClient = httpClient;

    public async ValueTask<List<TaskResponse>?> GetAll()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<TaskResponse>>("/api/tasks");
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async ValueTask<TaskResponse?> Get(string id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<TaskResponse>($"/api/tasks/{id}");
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
