using System.Net.Http.Json;
using TodoListBlazorWasm.Models.Dtos;
using TodoListBlazorWasm.Models.Requests.Task;
using TodoListBlazorWasm.Models.Responses;
using TodoListBlazorWasm.Models.SeedWork;
using YANLib;
using static Microsoft.AspNetCore.WebUtilities.QueryHelpers;
using static System.Text.Encoding;

namespace TodoListBlazorWasm.Services.Implements;

public sealed class TaskService : ITaskService
{
    private readonly HttpClient _httpClient;

    public TaskService(HttpClient httpClient) => _httpClient = httpClient;

    public async ValueTask<PagedList<TaskResponse>?> GetAll() => await _httpClient.GetFromJsonAsync<PagedList<TaskResponse>>("api/tasks");

    public async ValueTask<TaskResponse?> Get(string? id) => id.IsWhiteSpaceOrNull() ? default : await _httpClient.GetFromJsonAsync<TaskResponse>($"api/tasks/{id}");

    public async ValueTask<bool> Create(TaskCreateRequest? request)
    {
        if (request is null)
        {
            return default;
        }

        var res = await _httpClient.PostAsJsonAsync("api/tasks", request);

        return res.IsSuccessStatusCode && (await res.Content.ReadAsStringAsync()).Deserialize<TaskResponse>() is not null;
    }

    public async ValueTask<bool> Edit(string? id, TaskEditRequest? request)
    {
        if (id.IsWhiteSpaceOrNull() || request is null)
        {
            return default;
        }

        var res = await _httpClient.PutAsJsonAsync($"api/tasks/{id}", request);

        return res.IsSuccessStatusCode && (await res.Content.ReadAsStringAsync()).Deserialize<TaskResponse>() is not null;
    }

    public async ValueTask<bool> Update(string? id, TaskUpdateRequest? request)
    {
        if (id.IsWhiteSpaceOrNull() || request is null)
        {
            return default;
        }

        var res = await _httpClient.PatchAsync($"api/tasks/{id}", new StringContent(request.Serialize(), UTF8, "application/json-patch+json"));

        return res.IsSuccessStatusCode && (await res.Content.ReadAsStringAsync()).Deserialize<TaskResponse>() is not null;
    }

    public async ValueTask<bool> Delete(string? id) => (await _httpClient.DeleteAsync($"api/tasks/{id}")).IsSuccessStatusCode;

    public async ValueTask<PagedList<TaskResponse>?> Search(TasksSearchDto? tasksSearch)
    {
        if (tasksSearch is null)
        {
            return default;
        }
        else
        {
            var qryStrParam = new Dictionary<string, string>
            {
                [nameof(tasksSearch.PageNumber).ToLowerInvariant()] = tasksSearch.PageNumber.ToString()
            };

            if (tasksSearch.Name.IsNotWhiteSpaceAndNull())
            {
                qryStrParam.Add(nameof(tasksSearch.Name).ToLowerInvariant(), tasksSearch.Name);
            }

            if (tasksSearch.AssigneeId.HasValue)
            {
                qryStrParam.Add(nameof(tasksSearch.AssigneeId).ToLowerInvariant(), tasksSearch.AssigneeId.Value.ToString());
            }

            if (tasksSearch.Priority.HasValue)
            {
                qryStrParam.Add(nameof(tasksSearch.Priority).ToLowerInvariant(), tasksSearch.Priority.Value.ToString());
            }

            return await _httpClient.GetFromJsonAsync<PagedList<TaskResponse>>(AddQueryString("api/tasks/search", qryStrParam));
        }
    }
}
