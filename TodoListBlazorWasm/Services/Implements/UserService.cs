using System.Net.Http.Json;
using TodoListBlazorWasm.Models.Responses;

namespace TodoListBlazorWasm.Services.Implements;

public sealed class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient) => _httpClient = httpClient;

    public async ValueTask<List<UserResponse>?> GetAll() => await _httpClient.GetFromJsonAsync<List<UserResponse>>("api/users");

    public async ValueTask<UserResponse?> Get(string id) => await _httpClient.GetFromJsonAsync<UserResponse>($"api/users/{id}");
}
