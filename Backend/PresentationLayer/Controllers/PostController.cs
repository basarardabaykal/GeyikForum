using System.Security.Claims;
using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Services.ControllerServices;
using CoreLayer.Utilities.DataResults.Interfaces;
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
  [Authorize]
  public async Task<IActionResult> GetAll()
  {
    var result = await _postControllerService.GetAll();
    return StatusCode(result.StatusCode, result);
  }

  [HttpGet("get-page")]
  [Authorize]
  public async Task<IActionResult> GetPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
  {
    var result = await _postControllerService.GetPage(page, pageSize);
    return StatusCode(result.StatusCode, result);
  }

  [HttpPost("create-post")]
  [Authorize]
  public async Task<IActionResult> CreatePost([FromBody] PostDto postDto)
  {
    var result = await _postControllerService.CreatePost(postDto);
    return StatusCode(result.StatusCode, result);
  }

  [HttpPatch("vote-post")]
  [Authorize]
  public async Task<IActionResult> VotePost([FromBody] PostVoteDto postVoteDto)
  {
    var result = await _postControllerService.VotePost(postVoteDto);
    return StatusCode(result.StatusCode, result);
  }

  [HttpPatch("pin-post")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> PinPost([FromBody] PostPinRequestDto dto)
  {
    var result = await _postControllerService.SetPinStatus(dto);
    return StatusCode(result.StatusCode, result);
  }

  [HttpPatch("delete-post")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> DeletePost([FromBody] PostDeleteRequestDto dto)
  {
    var result = await _postControllerService.SetDeleteStatus(dto);
    return StatusCode(result.StatusCode, result);
  }
}