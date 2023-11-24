using Microsoft.EntityFrameworkCore;
using TodoListBlazorWasm.Api.Data;
using TodoListBlazorWasm.Api.Entities;

namespace TodoListBlazorWasm.Api.Repositories.Implements;

public sealed class UserRepository : IUserRepository
{
    private readonly TodoListDbContext _dbContext;

    public UserRepository(TodoListDbContext dbContext) => _dbContext = dbContext;

    public async ValueTask<IEnumerable<User>> GetAll() => await _dbContext.Users.Include(x => x.Tasks).AsNoTracking().ToArrayAsync();

    public async ValueTask<User?> Get(Guid id) => await _dbContext.Users.Include(x => x.Tasks).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async ValueTask<User?> Create(User user)
    {
        var ent = await _dbContext.Users.AddAsync(user);

        return await _dbContext.SaveChangesAsync() > 0 ? ent.Entity : default;
    }

    public async ValueTask<User?> Update(User user)
    {
        var ent = _dbContext.Users.Update(user);

        return await _dbContext.SaveChangesAsync() > 0 ? ent.Entity : default;
    }

    public async ValueTask<User?> Delete(User user)
    {
        var ent = _dbContext.Users.Remove(user);

        return await _dbContext.SaveChangesAsync() > 0 ? ent.Entity : default;
    }
}
