using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace MultilingualChat.Server.Services;

/// <summary>
///     For server-side translation using LLM
/// </summary>
public class TranslationService : ITranslationService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiUrl;

    public TranslationService(IOptions<TranslationServiceOptions> options, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.ApiKey;
        _apiUrl = options.Value.ApiUrl;
    }

    public async Task<string> TranslateAsync(string message, string sourceLanguage, string targetLanguage)
    {
        Console.WriteLine("api key is " + _apiKey);
        Console.WriteLine("api url is " + _apiUrl);

        var requestBody = new
        {
            messages = new[]
            {
                new
                {
                    role = "user",
                    content =
                        $"Translate the following text from {sourceLanguage} to {targetLanguage}: {message}. Always adhere to the following guidelines: Do not give me anything but the translated text. If there is no translation, return the original text, and do not add any additional text."
                }
            },
            model = "mixtral-8x7b-32768"
        };


        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            
        }

        var response = _httpClient.PostAsync(_apiUrl, content).Result;
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TranslationResponse>(responseBody);

        if (result?.Choices == null || result.Choices.Length == 0 || result.Choices[0].Message == null)
        {
            throw new Exception("No translation found");
        }
        
        return result.Choices[0].Message.Content?.Trim() ?? throw new Exception("Content is null");
    }
}

public class TranslationServiceOptions
{
    public string ApiKey { get; set; }
    public string ApiUrl { get; set; }
}

public class TranslationResponse
{
    public Choice[] Choices { get; set; }
}

public class Choice
{
    public Message Message { get; set; }
}

public class Message
{
    public string Role { get; set; }
    public string Content { get; set; }
}