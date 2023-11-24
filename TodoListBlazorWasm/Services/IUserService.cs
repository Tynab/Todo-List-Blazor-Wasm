using TodoListBlazorWasm.Models.Responses;

namespace TodoListBlazorWasm.Services;

public interface IUserService
{
    public ValueTask<List<UserResponse>?> GetAll();

    public ValueTask<UserResponse?> Get(string id);
}
