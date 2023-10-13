using Microsoft.EntityFrameworkCore;
using TodoListBlazorWasm.Api.Data;

namespace TodoListBlazorWasm.Api.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TodoListDbContext _dbContext;

    public TaskRepository(TodoListDbContext dbContext) => _dbContext = dbContext;

    public async ValueTask<IEnumerable<Entities.Task>> GetAll() => await _dbContext.Tasks.ToArrayAsync();

    public async ValueTask<Entities.Task?> Get(Guid id) => await _dbContext.Tasks.FindAsync(id);

    public async ValueTask<Entities.Task?> Insert(Entities.Task task)
    {
        var ent = await _dbContext.Tasks.AddAsync(task);

        return await _dbContext.SaveChangesAsync() > 0 ? ent.Entity : default;
    }

    public async ValueTask<Entities.Task?> Update(Entities.Task task)
    {
        var ent = _dbContext.Tasks.Update(task);

        return await _dbContext.SaveChangesAsync() > 0 ? ent.Entity : default;
    }

    public async ValueTask<Entities.Task?> Delete(Entities.Task task)
    {
        var ent = _dbContext.Tasks.Remove(task);

        return await _dbContext.SaveChangesAsync() > 0 ? ent.Entity : default;
    }
}
