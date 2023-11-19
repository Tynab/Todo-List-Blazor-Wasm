using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoListBlazorWasm.Models.Enums;

namespace TodoListBlazorWasm.Api.Entities;

public sealed class Task
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    public Priority Priority { get; set; }

    public Status Status { get; set; }

    public Guid? AssigneeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(AssigneeId))]
    public User? Assignee { get; set; }
}
