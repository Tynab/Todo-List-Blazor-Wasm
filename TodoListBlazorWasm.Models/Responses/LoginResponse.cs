namespace TodoListBlazorWasm.Models.Responses;

public sealed record LoginResponse
{
    public bool Success { get; set; }

    public string? Error { get; set; }

    public string? Token { get; set; }
}
