using ImageProcessor.Models;

namespace ImageProcessor.Services.Interfaces;

public interface IMetaDataService
{
    ImageData GetBasicMetadata(IFormFile stream);
}