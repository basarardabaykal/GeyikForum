using BusinessLayer.Dtos.Auth;
using BusinessLayer.Interfaces.Services.ControllerServices;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController : Controller
{
  private readonly IAuthControllerService _controllerService;

  public AuthController(IAuthControllerService controllerService)
  {
    _controllerService = controllerService;
  }
  
  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
  {
    var result = await _controllerService.Register(registerRequestDto);
    return StatusCode(result.StatusCode, result);
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
  {
    var result = await _controllerService.Login(loginRequestDto);
    return StatusCode(result.StatusCode, result);
  }
}