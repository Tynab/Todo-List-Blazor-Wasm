using TodoListBlazorWasm.Models.Requests;
using TodoListBlazorWasm.Models.Responses;

namespace TodoListBlazorWasm.Services;

public interface IAuthService
{
    public ValueTask<LoginResponse?> Login(LoginRequest request);

    public Task Logout();
}
