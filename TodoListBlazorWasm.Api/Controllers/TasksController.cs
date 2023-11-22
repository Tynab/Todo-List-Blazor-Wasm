using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TodoListBlazorWasm.Api.Repositories;
using TodoListBlazorWasm.Models.Requests.Task;
using TodoListBlazorWasm.Models.Responses;
using static System.DateTime;
using static TodoListBlazorWasm.Models.Enums.Status;

namespace TodoListBlazorWasm.Api.Controllers;

[Route("api/tasks")]
[ApiController]
public sealed class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TasksController(ITaskRepository taskRepository) => _taskRepository = taskRepository;

    [HttpGet]
    public async ValueTask<IActionResult> GetAll() => Ok((await _taskRepository.GetAll()).Select(x => new TaskResponse
    {
        Id = x.Id,
        Name = x.Name,
        Priority = x.Priority,
        Status = x.Status,
        CreatedAt = x.CreatedAt,
        UpdatedAt = x.UpdatedAt,
        Assignee = x.Assignee is null ? default : new UserResponse
        {
            Id = x.Assignee.Id,
            FirstName = x.Assignee.FirstName,
            LastName = x.Assignee.LastName,
        }
    }));

    [HttpGet("{id}")]
    public async ValueTask<IActionResult> Get(Guid id)
    {
        var ent = await _taskRepository.Get(id);

        return Ok(ent is null ? default : new TaskResponse
        {
            Id = ent.Id,
            Name = ent.Name,
            Priority = ent.Priority,
            Status = ent.Status,
            CreatedAt = ent.CreatedAt,
            UpdatedAt = ent.UpdatedAt,
            Assignee = ent.Assignee is null ? null : new UserResponse
            {
                Id = ent.Assignee.Id,
                FirstName = ent.Assignee.FirstName,
                LastName = ent.Assignee.LastName,
            }
        });
    }

    [HttpPost]
    public async ValueTask<IActionResult> Create([Required] TaskCreateRequest request) => !ModelState.IsValid ? BadRequest(ModelState) : Ok(await _taskRepository.Create(new Entities.Task
    {
        Id = request.Id,
        Name = request.Name,
        AssigneeId = request.AssigneeId,
        Priority = request.Priority,
        Status = Open,
        CreatedAt = Now
    }));

    [HttpPut("{id}")]
    public async ValueTask<IActionResult> Update(Guid id, [Required] TaskUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        else
        {
            var ent = await _taskRepository.Get(id);

            return ent is null
                ? NotFound($"{id} is not found!")
                : Ok(await _taskRepository.Update(new Entities.Task
                {
                    Id = id,
                    Name = request.Name,
                    AssigneeId = request.AssigneeId,
                    Priority = request.Priority,
                    Status = request.Status,
                    CreatedAt = ent.CreatedAt,
                    UpdatedAt = Now
                }));
        }
    }

    [HttpDelete("{id}")]
    public async ValueTask<IActionResult> Delete(Guid id)
    {
        var ent = await _taskRepository.Get(id);

        return ent is null ? NotFound($"{id} is not found!") : Ok(await _taskRepository.Delete(ent));
    }
}
