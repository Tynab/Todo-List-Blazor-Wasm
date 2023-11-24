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
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorageService;

    public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorageService = localStorageService;
    }

    public async ValueTask<LoginResponse?> Login(LoginRequest request)
    {
        var res = await _httpClient.PostAsJsonAsync("api/login", request);
        var rslt = (await res.Content.ReadAsStringAsync()).Deserialize<LoginResponse>();

        if (res.IsSuccessStatusCode && rslt is not null)
        {
            await _localStorageService.SetItemAsync("authToken", rslt.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(request.UserName!);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", rslt.Token);
        }

        return rslt;
    }

    public async Task Logout()
    {
        await _localStorageService.RemoveItemAsync("authToken");
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = default;
    }
}
