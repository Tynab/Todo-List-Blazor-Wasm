﻿using System.ComponentModel.DataAnnotations;
using TodoListBlazorWasm.Models.Enums;
using static System.Guid;
using static TodoListBlazorWasm.Models.Enums.Status;

namespace TodoListBlazorWasm.Models.Requests.Task;

public sealed class TaskCreateRequest
{
    public Guid Id { get; set; } = NewGuid();

    [Required(ErrorMessage = "Please enter your task name")]
    public string? Name { get; set; }

    public Guid? AssigneeId { get; set; }

    public Priority Priority { get; set; }

    public Status Status { get; set; } = Open;
}
