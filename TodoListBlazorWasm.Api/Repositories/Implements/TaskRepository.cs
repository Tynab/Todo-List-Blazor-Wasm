using Microsoft.EntityFrameworkCore;
using TodoListBlazorWasm.Api.Data;
using TodoListBlazorWasm.Models.Dtos;
using YANLib;

namespace TodoListBlazorWasm.Api.Repositories.Implements;

public sealed class TaskRepository : ITaskRepository
{
    private readonly TodoListDbContext _dbContext;

    public TaskRepository(TodoListDbContext dbContext) => _dbContext = dbContext;

    public async ValueTask<IEnumerable<Entities.Task>> GetAll() => await _dbContext.Tasks.Include(x => x.Assignee).OrderByDescending(x => x.CreatedAt).AsNoTracking().ToArrayAsync();

    public async ValueTask<Entities.Task?> Get(Guid id) => await _dbContext.Tasks.Include(x => x.Assignee).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async ValueTask<IEnumerable<Entities.Task>> Search(TasksSearchDto tasksSearch)
    {
        var qry = _dbContext.Tasks.Include(x => x.Assignee).AsQueryable();

        if (tasksSearch.Name!.IsNotWhiteSpaceAndNull())
        {
            qry = qry.Where(x => x.Name.Contains(tasksSearch.Name!));
        }

        if (tasksSearch.AssigneeId.HasValue)
        {
            qry = qry.Where(x => x.AssigneeId == tasksSearch.AssigneeId.Value);
        }

        if (tasksSearch.Priority.HasValue)
        {
            qry = qry.Where(x => x.Priority == tasksSearch.Priority.Value);
        }

        return await qry.OrderByDescending(x => x.CreatedAt).AsNoTracking().ToArrayAsync();
    }

    public async ValueTask<Entities.Task?> Create(Entities.Task task)
    {
        var ent = await _dbContext.Tasks.AddAsync(task);

        if (await _dbContext.SaveChangesAsync() > 0)
        {
            if (ent.Entity.AssigneeId is not null)
            {
                ent.Entity.Assignee = _dbContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == ent.Entity.AssigneeId);
            }

            return ent.Entity;
        }
        else
        {
            return default;
        }
    }

    public async ValueTask<Entities.Task?> Update(Entities.Task task)
    {
        var ent = _dbContext.Tasks.Update(task);

        if (await _dbContext.SaveChangesAsync() > 0)
        {
            if (ent.Entity.AssigneeId is not null)
            {
                ent.Entity.Assignee = _dbContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == ent.Entity.AssigneeId);
            }

            return ent.Entity;
        }
        else
        {
            return default;
        }
    }

    public async ValueTask<Entities.Task?> Delete(Entities.Task task)
    {
        var ent = _dbContext.Tasks.Remove(task);

        return await _dbContext.SaveChangesAsync() > 0 ? ent.Entity : default;
    }
}
