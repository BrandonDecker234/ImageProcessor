using ImageProcessor.Middleware;
using ImageProcessor.Models;
using ImageProcessor.Services;
using ImageProcessor.Services.Builders;
using ImageProcessor.Services.Builders.Interfaces;
using ImageProcessor.Services.Clients;
using ImageProcessor.Services.Clients.Interfaces;
using ImageProcessor.Services.Converters;
using ImageProcessor.Services.Converters.Interfaces;
using ImageProcessor.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "ImageProcessor API", 
        Version = "v1" 
    });
    // XML comments
    // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

builder.Services.AddScoped<ExceptionFilter>();

builder.Services.AddScoped(x =>
{
    var openRouter = builder.Configuration.GetSection("OpenRouter");
    return new AuthenticationConfigs(
        openRouter.GetSection("ApiKey").Value ?? throw new Exception("API key not found"),
        openRouter.GetSection("Domain").Value ?? throw new Exception("Domain not found")
    );
});

//Services
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IImageAnalysisService, ImageAnalysisService>();
builder.Services.AddScoped<IAiPromptBuilder, AiPromptBuilder>();
builder.Services.AddScoped<IAiRequestClientService,AiRequestClientService>();
builder.Services.AddScoped<IImageConverter, ImageConverter>();
builder.Services.AddScoped<IMetaDataService, MetaDataService>();
builder.Services.AddScoped<IJsonConverter, JsonConverter>();
builder.Services.AddScoped(x =>
{
    var configs = x.GetService<AuthenticationConfigs>();
    return Options.Create(configs);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        b => { b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ImageProcessor API v1");
            c.RoutePrefix = string.Empty; // serve UI at app root ("/")
        });
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowAllOrigins");

app.Run();