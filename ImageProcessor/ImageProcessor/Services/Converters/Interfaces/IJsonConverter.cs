namespace ImageProcessor.Services.Converters.Interfaces;

public interface IJsonConverter
{
    string NormalizeJson(string json);
    string Serialize<T>(T obj);
    T Deserialize<T>(string json);
}