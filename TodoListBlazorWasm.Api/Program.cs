using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoListBlazorWasm.Api.Data;
using TodoListBlazorWasm.Api.Extensions;
using TodoListBlazorWasm.Api.Options;
using TodoListBlazorWasm.Api.Repositories;
using TodoListBlazorWasm.Api.Repositories.Implements;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

builder.Services.AddDbContext<TodoListDbContext>((p, o) =>
{
    var databaseOptions = p.GetService<IOptions<DatabaseOptions>>()?.Value;

    _ = o.UseSqlServer(databaseOptions?.ConnectionString);
});

builder.Services.AddControllers();
builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", b => b.SetIsOriginAllowed((h) => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

var app = builder.Build();

app.MigrateDbContext<TodoListDbContext>((c, s) => new TodoListDbContextSeed().SeedAsync(c, s.GetService<ILogger<TodoListDbContextSeed>>()!).Wait());

if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();
