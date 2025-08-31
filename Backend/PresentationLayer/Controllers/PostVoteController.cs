using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Services.ControllerServices;
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
  public async Task<IActionResult> GetAll()
  {
    var response = await _postVoteControllerService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [HttpPost("create-post-vote")]
  public async Task<IActionResult> CreatePostVote([FromBody] PostVoteDto postVote)
  {
    var response = await _postVoteControllerService.CreatePostVote(postVote);
    return StatusCode(response.StatusCode, response);
  }
}