using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TodoListBlazorWasm.Api.Entities;
using TodoListBlazorWasm.Api.Repositories;
using TodoListBlazorWasm.Models.Requests.User;
using TodoListBlazorWasm.Models.Responses;
using YANLib;
using static System.Guid;

namespace TodoListBlazorWasm.Api.Controllers;

[Route("api/users")]
[ApiController]
public sealed class UserController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public UserController(IUserRepository repository) => _repository = repository;

    [HttpGet]
    public async ValueTask<IActionResult> GetAll() => Ok((await _repository.GetAll()).Select(x => new UserResponse
    {
        Id = x.Id,
        FirstName = x.FirstName,
        LastName = x.LastName
    }));

    [HttpGet("{id}")]
    public async ValueTask<IActionResult> Get(Guid id)
    {
        var ent = await _repository.Get(id);

        return Ok(ent is null ? default : new UserResponse
        {
            Id = ent.Id,
            FirstName = ent.FirstName,
            LastName = ent.LastName
        });
    }

    [HttpPost]
    public async ValueTask<IActionResult> Create([Required] UserCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var ent = new User
        {
            Id = NewGuid(),
            Email = request.Email,
            NormalizedEmail = request.Email.ToUpperInvariant(),
            PhoneNumber = request.PhoneNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            NormalizedUserName = request.UserName.ToUpperInvariant()
        };

        ent.PasswordHash = _passwordHasher.HashPassword(ent, request.Password);

        var rslt = await _repository.Create(ent);

        return rslt is null ? Problem() : Ok(new UserResponse
        {
            Id = rslt.Id,
            FirstName = rslt.FirstName,
            LastName = rslt.LastName
        });
    }

    [HttpPut("{id}")]
    public async ValueTask<IActionResult> Edit(Guid id, UserEditRequest request)
    {
        var ent = await _repository.Get(id);

        if (ent is null)
        {
            return NotFound($"{id} is not found!");
        }

        ent.FirstName = request.FirstName!;
        ent.LastName = request.LastName!;
        ent.Email = request.Email;
        ent.NormalizedEmail = request.Email!.ToUpperInvariant();
        ent.PhoneNumber = request.PhoneNumber;
        ent.PasswordHash = _passwordHasher.HashPassword(ent, request.Password!);

        var rslt = await _repository.Update(ent);

        return rslt is null ? Problem() : Ok(new UserResponse
        {
            Id = rslt.Id,
            FirstName = rslt.FirstName,
            LastName = rslt.LastName
        });
    }

    [HttpPatch("{id}")]
    public async ValueTask<IActionResult> Update(Guid id, UserUpdateRequest request)
    {
        var ent = await _repository.Get(id);

        if (ent is null)
        {
            return NotFound($"{id} is not found!");
        }

        if (request.FirstName!.IsNotWhiteSpaceAndNull())
        {
            ent.FirstName = request.FirstName!;
        }

        if (request.LastName!.IsNotWhiteSpaceAndNull())
        {
            ent.LastName = request.LastName!;
        }

        if (request.Email!.IsNotWhiteSpaceAndNull())
        {
            ent.Email = request.Email;
            ent.NormalizedEmail = request.Email!.ToUpperInvariant();
        }

        if (request.PhoneNumber!.IsNotWhiteSpaceAndNull())
        {
            ent.PhoneNumber = request.PhoneNumber;
        }

        if (request.Password!.IsNotWhiteSpaceAndNull())
        {
            ent.PasswordHash = _passwordHasher.HashPassword(ent, request.Password!);
        }

        var rslt = await _repository.Update(ent);

        return rslt is null ? Problem() : Ok(new UserResponse
        {
            Id = rslt.Id,
            FirstName = rslt.FirstName,
            LastName = rslt.LastName
        });
    }

    [HttpDelete("{id}")]
    public async ValueTask<IActionResult> Delete(Guid id)
    {
        var ent = await _repository.Get(id);

        return ent is null ? NotFound($"{id} is not found!") : Ok(await _repository.Delete(ent));
    }
}
