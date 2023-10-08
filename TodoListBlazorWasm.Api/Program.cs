using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoListBlazorWasm.Api.Data;
using TodoListBlazorWasm.Api.Extensions;
using TodoListBlazorWasm.Api.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

builder.Services.AddDbContext<TodoListDbContext>((p, o) =>
{
    var databaseOptions = p.GetService<IOptions<DatabaseOptions>>()?.Value;

    _ = o.UseSqlServer(databaseOptions?.ConnectionString);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MigrateDbContext<TodoListDbContext>((c, s) =>
{
    var logger = s.GetService<ILogger<TodoListDbContextSeed>>();

    new TodoListDbContextSeed().SeedAsync(c, logger!).Wait();
});

if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
