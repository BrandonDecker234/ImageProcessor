using ImageProcessor.Models;

namespace ImageProcessor.Services.Interfaces;

public interface IMetaDataService
{
    /// <summary>
    /// Extracts basic metadata from an uploaded image file.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> representing the uploaded image file.</param>
    /// <returns>An <see cref="ImageData"/> object containing the extracted metadata.
    /// The specific metadata fields included depend on the image file format and its embedded data.</returns>
    ImageData GetBasicMetadata(IFormFile stream);
}