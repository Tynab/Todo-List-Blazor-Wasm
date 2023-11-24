using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TodoListBlazorWasm.Api.Repositories;
using TodoListBlazorWasm.Models.Requests.Task;
using TodoListBlazorWasm.Models.Responses;
using YANLib;
using static System.DateTime;
using static TodoListBlazorWasm.Models.Enums.Status;

namespace TodoListBlazorWasm.Api.Controllers;

[Route("api/tasks")]
[ApiController]
public sealed class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;

    public TasksController(ITaskRepository repository) => _repository = repository;

    [HttpGet]
    public async ValueTask<IActionResult> GetAll() => Ok((await _repository.GetAll()).Select(x => new TaskResponse
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
        var ent = await _repository.Get(id);

        return Ok(ent is null ? default : new TaskResponse
        {
            Id = ent.Id,
            Name = ent.Name,
            Priority = ent.Priority,
            Status = ent.Status,
            CreatedAt = ent.CreatedAt,
            UpdatedAt = ent.UpdatedAt,
            Assignee = ent.Assignee is null ? default : new UserResponse
            {
                Id = ent.Assignee.Id,
                FirstName = ent.Assignee.FirstName,
                LastName = ent.Assignee.LastName,
            }
        });
    }

    [HttpPost]
    public async ValueTask<IActionResult> Create([Required] TaskCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var rslt = await _repository.Create(new Entities.Task
        {
            Id = request.Id,
            Name = request.Name,
            AssigneeId = request.AssigneeId,
            Priority = request.Priority,
            Status = Open,
            CreatedAt = Now
        });

        return rslt is null ? Problem() : Ok(new TaskResponse
        {
            Id = rslt!.Id,
            Name = rslt.Name,
            Priority = rslt.Priority,
            Status = rslt.Status,
            CreatedAt = rslt.CreatedAt,
            UpdatedAt = rslt.UpdatedAt,
            Assignee = rslt.Assignee is null ? default : new UserResponse
            {
                Id = rslt.Assignee.Id,
                FirstName = rslt.Assignee.FirstName,
                LastName = rslt.Assignee.LastName,
            }
        });
    }

    [HttpPatch("{id}")]
    public async ValueTask<IActionResult> Update(Guid id, [Required] TaskUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var ent = await _repository.Get(id);

        if (ent is null)
        {
            return NotFound($"{id} is not found!");
        }

        if (request.Name!.IsNotWhiteSpaceAndNull())
        {
            ent.Name = request.Name!;
        }

        if (request.AssigneeId.HasValue)
        {
            ent.AssigneeId = request.AssigneeId.Value;
        }

        if (request.Priority.HasValue)
        {
            ent.Priority = request.Priority.Value;
        }

        if (request.Status.HasValue)
        {
            ent.Status = request.Status.Value;
        }

        var rslt = await _repository.Update(ent);

        return rslt is null ? Problem() : Ok(new TaskResponse
        {
            Id = rslt.Id,
            Name = rslt.Name,
            Priority = rslt.Priority,
            Status = rslt.Status,
            CreatedAt = rslt.CreatedAt,
            UpdatedAt = rslt.UpdatedAt,
            Assignee = rslt.Assignee is null ? default : new UserResponse
            {
                Id = rslt.Assignee.Id,
                FirstName = rslt.Assignee.FirstName,
                LastName = rslt.Assignee.LastName,
            }
        });
    }

    [HttpDelete("{id}")]
    public async ValueTask<IActionResult> Delete(Guid id)
    {
        var ent = await _repository.Get(id);

        return ent is null ? NotFound($"{id} is not found!") : Ok(await _repository.Delete(ent));
    }
}
