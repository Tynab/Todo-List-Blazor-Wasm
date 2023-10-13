namespace TodoListBlazorWasm.Api.Repositories;

public interface ITaskRepository
{
    public ValueTask<IEnumerable<Entities.Task>> GetAll();
    public ValueTask<Entities.Task?> Get(Guid id);
    public ValueTask<Entities.Task?> Insert(Entities.Task task);
    public ValueTask<Entities.Task?> Update(Entities.Task task);
    public ValueTask<Entities.Task?> Delete(Entities.Task task);
}
