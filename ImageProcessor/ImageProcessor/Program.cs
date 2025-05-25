using ImageProcessor.Models;
using ImageProcessor.Services;
using ImageProcessor.Services.Clients;
using ImageProcessor.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Services
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();

builder.Services.AddScoped(x =>
{
    var openRouter = builder.Configuration.GetSection("OpenRouter");
    return new AuthenticationConfigs(
        openRouter.GetSection("ApiKey").Value ?? throw new Exception("API key not found"),
        openRouter.GetSection("Domain").Value ?? throw new Exception("Domain not found")
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();