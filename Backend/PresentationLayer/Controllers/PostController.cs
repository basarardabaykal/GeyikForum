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

  [HttpGet("GetAll")]
  public async Task<IActionResult> GetAll()
  {
    var result = await _postControllerService.GetAll();
    return StatusCode(result.StatusCode, result);
  }
}