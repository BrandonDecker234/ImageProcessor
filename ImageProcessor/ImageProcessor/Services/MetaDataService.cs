using ImageProcessor.Models;
using ImageProcessor.Services.Interfaces;
using MetadataExtractor;


namespace ImageProcessor.Services;

public class MetaDataService : IMetaDataService
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

    public ImageData GetBasicMetadata(IFormFile file)
    {
        var stream = file.OpenReadStream();
        return GetBasicMetadata(stream);
    }
}