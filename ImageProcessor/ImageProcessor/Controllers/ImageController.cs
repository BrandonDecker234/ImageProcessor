using ImageProcessor.Services.Converters.Interfaces;
using ImageProcessor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController(IImageService imageService, ILogger<ImageController> logger) : ControllerBase
{
    [HttpPost("rotate")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult>  RotateImage(
        IFormFile file,
        int degrees)
    {
        try
        {
            var result = await imageService.RotateAsync(file, degrees);
            return File(result, file.ContentType, imageService.SetDownloadFileName("uploaded_image"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during image analysis");
            return StatusCode(StatusCodes.Status500InternalServerError, "Image analysis failed.");
        }
    }
    [HttpPost("grayscale")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult>  ApplyGreyScale(
        IFormFile file)
    {
        try
        {
            var result = await imageService.ApplyGrayscaleAsync(file);
            return File(result, file.ContentType, imageService.SetDownloadFileName("uploaded_image"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during image analysis");
            return StatusCode(StatusCodes.Status500InternalServerError, "Image analysis failed.");
        }
    }

    [HttpPost("resize")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ResizeImage(
        IFormFile file,
        int width)
    {
        try
        {
            var result = await imageService.ResizeAsync(file, width, 0);
            return File(result, file.ContentType, imageService.SetDownloadFileName("uploaded_image"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during image analysis");
            return StatusCode(StatusCodes.Status500InternalServerError, "Image analysis failed.");
        }
    }
}