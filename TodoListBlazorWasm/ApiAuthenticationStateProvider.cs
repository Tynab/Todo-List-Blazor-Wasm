﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using YANLib;
using static System.Security.Claims.ClaimTypes;
using static System.Text.Json.JsonSerializer;
using static System.Threading.Tasks.Task;

namespace TodoListBlazorWasm;

public sealed class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var tk = await _localStorageService.GetItemAsync<string>("authToken");

        if (tk.IsWhiteSpaceOrNull())
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tk);

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(tk), "jwt")));
    }

    public void MarkUserAsAuthenticated(string usernam) => NotifyAuthenticationStateChanged(FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(Name, usernam)
    }, "apiauth")))));

    public void MarkUserAsLoggedOut() => NotifyAuthenticationStateChanged(FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));

    private static IEnumerable<Claim>? ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var keyValPrs = Deserialize<Dictionary<string, object>>(ParseBase64WithoutPadding(jwt.Split('.')[1]));

        if (keyValPrs!.TryGetValue(Role, out var roles))
        {
            if (roles is not null)
            {
                var sRoles = roles.ToString();

                if (sRoles!.Trim().StartsWith("["))
                {
                    foreach (var role in sRoles.Deserialize<string[]>()!)
                    {
                        claims.Add(new Claim(Role, role));
                    }
                }
                else
                {
                    claims.Add(new Claim(Role, sRoles));
                }

                _ = keyValPrs.Remove(Role);
            }

            claims.AddRange(keyValPrs.Select(x => new Claim(x.Key, x.Value.ToString()!)));
        }

        return default;
    }

    private static byte[] ParseBase64WithoutPadding(string base64) => Convert.FromBase64String($"{base64}{(base64.Length % 4) switch
    {
        2 => "==",
        3 => "=",
        _ => string.Empty
    }}");
}
