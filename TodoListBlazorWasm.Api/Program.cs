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

_ = builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
_ = builder.Services.AddDbContext<TodoListDbContext>((p, o) => o.UseSqlServer(p.GetService<IOptions<DatabaseOptions>>()?.Value.ConnectionString));
_ = builder.Services.AddControllers();
_ = builder.Services.AddEndpointsApiExplorer();
_ = builder.Services.AddSwaggerGen();
_ = builder.Services.AddTransient<IUserRepository, UserRepository>();
_ = builder.Services.AddTransient<ITaskRepository, TaskRepository>();
_ = builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", b => b.SetIsOriginAllowed((h) => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
_ = builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<TodoListDbContext>();

_ = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
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

_ = app.MigrateDbContext<TodoListDbContext>((c, s) => new TodoListDbContextSeed().SeedAsync(s.GetService<ILogger<TodoListDbContextSeed>>(), c).Wait());
_ = app.UseHttpsRedirection();
_ = app.UseCors("CorsPolicy");
_ = app.UseAuthentication();
_ = app.UseAuthorization();
_ = app.MapControllers();
app.Run();
