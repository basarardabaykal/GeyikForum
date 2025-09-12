using System.Security.Claims;
using BusinessLayer.Dtos.Auth;
using BusinessLayer.Interfaces.Services.ControllerServices;
using Microsoft.AspNetCore.Authorization;
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

  [HttpPatch("confirm-email")]
  public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequestDto confirmEmailRequestDto)
  {
    var result = await _controllerService.ConfirmEmail(confirmEmailRequestDto);
    return StatusCode(result.StatusCode, result);
  }

  [HttpGet("get-current-user")]
  [Authorize]
  public async Task<IActionResult> GetCurrentUser()
  {
    var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var result = await _controllerService.GetCurrentUser(uid);
    return StatusCode(result.StatusCode, result);
  }
}