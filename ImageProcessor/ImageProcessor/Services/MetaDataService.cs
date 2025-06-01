using ImageProcessor.Models;
using ImageProcessor.Services.Interfaces;
using MetadataExtractor;


namespace ImageProcessor.Services;

/// <summary>
/// Provides services for extracting metadata from image files.
/// This service implements the <see cref="IMetaDataService"/> interface.
/// </summary>
/// <param name="logger">The logger instance.</param>
public class MetaDataService(ILogger<IMetaDataService> logger) : IMetaDataService
{
    private static ImageData GetBasicMetadata(Stream stream)
    {
        var imageData = new ImageData();
        
        stream.Position = 0;
        var directories = ImageMetadataReader.ReadMetadata(stream);

        foreach (var dir in directories)
        foreach (var tag in dir.Tags) imageData.Metadata[$"{dir.Name}/{tag.Name}"] = tag.Description ?? "";
        
        return imageData;
    }

    /// <summary>
    /// Extracts basic metadata from an image.
    /// It reads all available metadata directories and tags, storing them in an <see cref="ImageData"/> object.
    /// </summary>
    /// <param name="file">The <see cref="IFormFile"/> the uploaded image file.</param>
    /// <returns>An <see cref="ImageData"/> object containing the extracted metadata.</returns>
    public ImageData GetBasicMetadata(IFormFile file)
    {
        var stream = file.OpenReadStream();
        logger.LogInformation($"Retrieving metadata for stream {file.FileName}");
        return GetBasicMetadata(stream);
    }
}