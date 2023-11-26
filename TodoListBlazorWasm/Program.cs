using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TodoListBlazorWasm;
using TodoListBlazorWasm.Services;
using TodoListBlazorWasm.Services.Implements;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAuthorizationCore();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITaskService, TaskService>();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

builder.Services.AddScoped(s => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["BackendApiUrl"] ?? "https://localhost:7257/")
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
await builder.Build().RunAsync();
