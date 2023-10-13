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
    public async ValueTask<IActionResult> Insert(Entities.Task task) => Ok(await _taskRepository.Insert(task));

    [HttpPut]
    public async ValueTask<IActionResult> Update(Entities.Task task) => Ok(await _taskRepository.Update(task));

    [HttpDelete]
    public async ValueTask<IActionResult> Delete(Entities.Task task) => Ok(await _taskRepository.Delete(task));
}
