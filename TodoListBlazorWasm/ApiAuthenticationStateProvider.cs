using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using YANLib;
using static System.Convert;
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

    public void MarkUserAsAuthenticated(string email) => NotifyAuthenticationStateChanged(FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(Name, email)
    }, "apiauth")))));

    public void MarkUserAsLoggedOut() => NotifyAuthenticationStateChanged(FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var rslt = new List<Claim>();

        var keyValPrs = Deserialize<Dictionary<string, object>>(ParseBase64WithoutPadding(jwt.Split('.')[1]), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        if (keyValPrs is not null && keyValPrs.TryGetValue(Role, out var rawRoles))
        {
            if (rawRoles is not null)
            {
                var sRoles = rawRoles.ToString();

                if (sRoles!.Trim().StartsWith("["))
                {
                    var roles = sRoles.Deserialize<string[]>();

                    if (roles?.Length > 0)
                    {
                        foreach (var role in roles)
                        {
                            rslt.Add(new Claim(Role, role));
                        }
                    }
                }
                else
                {
                    rslt.Add(new Claim(Role, sRoles));
                }

                _ = keyValPrs.Remove(Role);
            }

            rslt.AddRange(keyValPrs.Select(x => new Claim(x.Key, x.Value.ToString()!)));
        }

        return rslt;
    }

    private static byte[] ParseBase64WithoutPadding(string base64) => FromBase64String($"{base64}{(base64.Length % 4) switch
    {
        2 => "==",
        3 => "=",
        _ => string.Empty
    }}");
}
