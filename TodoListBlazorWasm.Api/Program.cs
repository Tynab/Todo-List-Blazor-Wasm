using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoListBlazorWasm.Api.Data;
using TodoListBlazorWasm.Api.Entities;
using TodoListBlazorWasm.Api.Extensions;
using TodoListBlazorWasm.Api.Options;
using TodoListBlazorWasm.Api.Repositories;
using TodoListBlazorWasm.Api.Repositories.Implements;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
builder.Services.AddDbContext<TodoListDbContext>((p, o) => o.UseSqlServer(p.GetService<IOptions<DatabaseOptions>>()?.Value.ConnectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", b => b.SetIsOriginAllowed((h) => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<TodoListDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["JwtIssuer"],
    ValidAudience = builder.Configuration["JwtAudience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"] ?? string.Empty))
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.MigrateDbContext<TodoListDbContext>((c, s) => new TodoListDbContextSeed().SeedAsync(s.GetService<ILogger<TodoListDbContextSeed>>(), c).Wait());
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
