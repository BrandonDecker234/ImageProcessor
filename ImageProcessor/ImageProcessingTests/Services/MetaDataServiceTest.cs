using ImageProcessor.Services;

namespace ImageProcessingTests.Services;

public class MetaDataServiceTest
{
    private readonly MetaDataService _metaDataService = new();

    [Fact]
    public void GetMetaData_Success_ReturnsMetaDataObject()
    {
        //TODO: Change this later so not accessing my local storage directly
        const string path = @"C:\Users\brand\Development\ImageProcessor\ImageProcessor\ImageProcessingTests\Images\field.jpg";
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        
        var results = _metaDataService.GetBasicMetadata(stream);
        Assert.NotNull(results);
        Assert.NotEmpty(results.Metadata);
    }
}