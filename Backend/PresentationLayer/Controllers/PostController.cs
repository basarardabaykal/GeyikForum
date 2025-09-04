using BusinessLayer.Interfaces.Services.ControllerServices;
using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : Controller
{
  private readonly IPostControllerService _postControllerService;
  
  public PostController(IPostControllerService postControllerService) 
    {
      _postControllerService = postControllerService;
    }

  [HttpGet("get-all")]
  public async Task<IActionResult> GetAll()
  {
    var result = await _postControllerService.GetAll();
    return StatusCode(result.StatusCode, result);
  }

  [HttpPost("create-post")]
  public async Task<IActionResult> CreatePost([FromBody] PostDto postDto)
  {
    var result = await _postControllerService.CreatePost(postDto);
    return StatusCode(result.StatusCode, result);
  }

  [HttpPatch("vote-post")]
  public async Task<IActionResult> VotePost([FromBody] PostVoteDto postVoteDto)
  {
    var result = await _postControllerService.VotePost(postVoteDto);
    return StatusCode(result.StatusCode, result);
  }
  
}