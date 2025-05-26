using System.ComponentModel.DataAnnotations;

namespace ImageProcessor.Models;

public class ImageData
{
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Key/value pairs for metadata
    /// </summary>
    public IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    
    // public float Height { get; set; }
    // public float Width { get; set; }
    //
    // public string FileName { get; set; }
    //
    // public string? FileExtension { get; set; }
}

//TODO: Update this later so everything isn't in a dictionary