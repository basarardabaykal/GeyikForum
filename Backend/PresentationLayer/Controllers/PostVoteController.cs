using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Services.ControllerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]

public class PostVoteController : Controller
{
  private readonly IPostVoteControllerService _postVoteControllerService;

  public PostVoteController(IPostVoteControllerService postVoteControllerService)
  {
    _postVoteControllerService = postVoteControllerService;
  }

  [HttpGet("get-all")]
  [Authorize]
  public async Task<IActionResult> GetAll()
  {
    var response = await _postVoteControllerService.GetAll();
    return StatusCode(response.StatusCode, response);
  }
  
}