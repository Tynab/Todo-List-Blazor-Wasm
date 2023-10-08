using Microsoft.Extensions.Options;

namespace TodoListBlazorWasm.Api.Options;

public sealed class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private readonly IConfiguration _configuration;

    public DatabaseOptionsSetup(IConfiguration configuration) => _configuration = configuration;

    public void Configure(DatabaseOptions options) => options.ConnectionString = _configuration.GetConnectionString("Default")!;
}
