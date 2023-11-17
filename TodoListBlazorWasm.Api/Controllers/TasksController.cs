using Microsoft.AspNetCore.Mvc;
using TodoListBlazorWasm.Api.Repositories;

namespace TodoListBlazorWasm.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TasksController(ITaskRepository taskRepository) => _taskRepository = taskRepository;

    [HttpGet]
    public async ValueTask<IActionResult> GetAll() => Ok(await _taskRepository.GetAll());

    [HttpGet("{id}")]
    public async ValueTask<IActionResult> Get(Guid id) => Ok(await _taskRepository.Get(id));

    [HttpPost]
    public async ValueTask<IActionResult> Insert(Entities.Task task) => !ModelState.IsValid ? BadRequest(ModelState) : Ok(await _taskRepository.Insert(task));

    [HttpPut]
    public async ValueTask<IActionResult> Update(Entities.Task task) => !ModelState.IsValid
        ? BadRequest(ModelState)
        : await _taskRepository.Get(task.Id) is null
        ? NotFound($"{task.Id} is not found!")
        : Ok(await _taskRepository.Update(task));

    [HttpDelete("{id}")]
    public async ValueTask<IActionResult> Delete(Guid id)
    {
        var ent = await _taskRepository.Get(id);

        return ent is null ? NotFound($"{id} is not found!") : Ok(await _taskRepository.Delete(ent));
    }
}
