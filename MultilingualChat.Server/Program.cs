using System.Text.Json;
using MultilingualChat.Server.Hubs;
using MultilingualChat.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserManager, UserManager>();
builder.Services.AddSingleton<ITranslationService, TranslationService>();
builder.Services.AddHttpClient<ITranslationService, TranslationService>();

// Configurations
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

builder.Services.Configure<JsonSerializerOptions>(options => { options.PropertyNameCaseInsensitive = true; });

builder.Services.Configure<LargeLanguageModelConfig>(builder.Configuration.GetSection("LargeLanguageModelConfig"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.RoutePrefix = "swagger"; });
}

app.UseHttpsRedirection();

app.MapHub<ChatHub>("/chat");

app.Run();