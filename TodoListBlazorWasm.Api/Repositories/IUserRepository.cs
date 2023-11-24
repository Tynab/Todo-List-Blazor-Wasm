using TodoListBlazorWasm.Api.Entities;

namespace TodoListBlazorWasm.Api.Repositories;

public interface IUserRepository
{
    public ValueTask<IEnumerable<User>> GetAll();

    public ValueTask<User?> Get(Guid id);

    public ValueTask<User?> Create(User user);

    public ValueTask<User?> Update(User user);

    public ValueTask<User?> Delete(User user);
}
