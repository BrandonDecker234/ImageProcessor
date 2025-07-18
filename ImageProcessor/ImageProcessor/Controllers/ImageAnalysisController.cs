﻿using System.ComponentModel;
using ImageProcessor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageAnalysisController(
    IImageAnalysisService analysisService, 
    ILogger<ImageAnalysisController> logger,
    IMetaDataService metaDataService)
    : ControllerBase
{
    [HttpPost("analyze")]
    [Consumes("multipart/form-data")]
    [Description("Analyzes an image to identify subject matters")]
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
    
    [HttpPost("meta-data")]
    [Consumes("multipart/form-data")]
    [Description("Retrieves metadata from image")]
    public Task<IActionResult>  GetMetaData(
        IFormFile file,
        string model)
    {
        try
        {
            var result = metaDataService.GetBasicMetadata(file);
            return Task.FromResult<IActionResult>(Ok(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during image analysis");
            return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status500InternalServerError, "Image analysis failed."));
        }
    }
}