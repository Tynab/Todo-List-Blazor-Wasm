using System.Net.Http.Json;
using TodoListBlazorWasm.Models.Responses;
using YANLib;

namespace TodoListBlazorWasm.Services.Implements;

public sealed class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient) => _httpClient = httpClient;

    public async ValueTask<List<UserResponse>?> GetAll() => await _httpClient.GetFromJsonAsync<List<UserResponse>>("api/users");

    public async ValueTask<UserResponse?> Get(string? id) => id.IsWhiteSpaceOrNull() ? default : await _httpClient.GetFromJsonAsync<UserResponse>($"api/users/{id}");
}
