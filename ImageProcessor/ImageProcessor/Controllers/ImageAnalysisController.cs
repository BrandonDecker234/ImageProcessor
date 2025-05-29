using ImageProcessor.Models;
using ImageProcessor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageAnalysisController(IImageAnalysisService analysisService, ILogger<ImageAnalysisController> logger)
    : ControllerBase
{
    [HttpPost("analyze")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult>  GetImageAnalysis(
        IFormFile file,
        string model)
    {
        try
        {
            var result = await analysisService.AnalyzeImage(file, model);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during image analysis");
            return StatusCode(StatusCodes.Status500InternalServerError, "Image analysis failed.");
        }
    }
}