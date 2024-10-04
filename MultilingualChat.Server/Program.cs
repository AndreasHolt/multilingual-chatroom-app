using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using MultilingualChat.Server.Hubs;
using MultilingualChat.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserManager, UserManager>();
builder.Services.AddSingleton<ITranslationService, TranslationService>();
builder.Services.AddHttpClient<ITranslationService, TranslationService>();

// Configure Translation Service
var apiKey = Environment.GetEnvironmentVariable("TRANSLATION_API_KEY");
if (string.IsNullOrEmpty(apiKey))
{
    throw new ArgumentException("TRANSLATION_API_KEY environment variable is not set");
}

builder.Services.Configure<TranslationServiceOptions>(options =>
{
    options.ApiKey = apiKey;
    options.ApiUrl = "https://api.groq.com/openai/v1/chat/completions";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.RoutePrefix = "swagger"; });
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapHub<ChatHub>("/chat");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}