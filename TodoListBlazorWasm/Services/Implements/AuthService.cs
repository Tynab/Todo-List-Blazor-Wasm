using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TodoListBlazorWasm.Models.Requests;
using TodoListBlazorWasm.Models.Responses;
using YANLib;

namespace TodoListBlazorWasm.Services.Implements;

public sealed class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async ValueTask<LoginResponse?> Login(LoginRequest request)
    {
        var rslt = await _httpClient.PostAsJsonAsync("/api/login", request);
        var res = (await rslt.Content.ReadAsStringAsync()).Deserialize<LoginResponse>();

        if (rslt.IsSuccessStatusCode)
        {
            await _localStorageService.SetItemAsync("authToken", res!.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(request.UserName!);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", res.Token);
        }

        return res;
    }

    public async Task Logout()
    {
        await _localStorageService.RemoveItemAsync("authToken");
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
