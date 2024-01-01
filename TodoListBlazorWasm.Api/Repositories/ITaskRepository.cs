using TodoListBlazorWasm.Models.Dtos;
using TodoListBlazorWasm.Models.SeedWork;

namespace TodoListBlazorWasm.Api.Repositories;

public interface ITaskRepository
{
    public ValueTask<PagedList<Entities.Task>> GetAll();

    public ValueTask<Entities.Task?> Get(Guid id);

    public ValueTask<PagedList<Entities.Task>> Search(TasksSearchDto tasksSearch);

    public ValueTask<Entities.Task?> Create(Entities.Task task);

    public ValueTask<Entities.Task?> Update(Entities.Task task);

    public ValueTask<Entities.Task?> Delete(Entities.Task task);
}
