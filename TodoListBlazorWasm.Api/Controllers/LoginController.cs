using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoListBlazorWasm.Api.Entities;
using TodoListBlazorWasm.Models.Requests;
using TodoListBlazorWasm.Models.Responses;
using YANLib;
using static Microsoft.IdentityModel.Tokens.SecurityAlgorithms;
using static System.DateTime;
using static System.Security.Claims.ClaimTypes;
using static System.Text.Encoding;

namespace TodoListBlazorWasm.Api.Controllers;

[Route("api/login")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly SignInManager<User> _signInManager;

    public LoginController(IConfiguration configuration, SignInManager<User> signInManager)
    {
        _configuration = configuration;
        _signInManager = signInManager;
    }

    [HttpPost]
    public async ValueTask<IActionResult> Login([Required] LoginRequest request) => !(await _signInManager.PasswordSignInAsync(request.UserName ?? string.Empty, request.Password ?? string.Empty, false, false)).Succeeded
        ? BadRequest(new LoginResponse
        {
            Success = false,
            Error = "Username or Password are invalid"
        })
        : Ok(new LoginResponse
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(_configuration["JwtIssuer"], _configuration["JwtAudience"], new[]
            {
                new Claim(Name, request.UserName)
            }, expires: Now.AddDays(_configuration["JwtExpiryInDays"]!.ToInt(1)), signingCredentials: new SigningCredentials(new SymmetricSecurityKey(UTF8.GetBytes(_configuration["JwtSecurityKey"] ?? string.Empty)), HmacSha256)))
        });
}
