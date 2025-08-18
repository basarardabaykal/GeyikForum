using BusinessLayer.Interfaces.Services.ControllerServices;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers;

public class UserController : Controller
{
  private readonly IUserControllerService _userControllerService;

  public UserController(IUserControllerService userControllerService)
  {
    _userControllerService = userControllerService;
  }

  [HttpGet("GetAll")]
  public async Task<IActionResult> GetAll()
  {
    var result = await _userControllerService.GetAll();
    return StatusCode(result.StatusCode, result);
  }
}