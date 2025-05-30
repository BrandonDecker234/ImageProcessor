using Microsoft.AspNetCore.Mvc;

namespace ImageProcessor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    // GET
    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}