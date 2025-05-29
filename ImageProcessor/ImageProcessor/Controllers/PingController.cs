using Microsoft.AspNetCore.Mvc;

namespace ImageProcessor.Controllers;

public class PingController : ControllerBase
{
    // GET
    public IActionResult Index()
    {
        return Ok();
    }
}