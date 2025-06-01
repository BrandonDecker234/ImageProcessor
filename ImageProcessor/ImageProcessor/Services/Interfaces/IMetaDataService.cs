using ImageProcessor.Models;

namespace ImageProcessor.Services.Interfaces;

public interface IMetaDataService
{
    ImageData GetBasicMetadata(Stream stream);
}