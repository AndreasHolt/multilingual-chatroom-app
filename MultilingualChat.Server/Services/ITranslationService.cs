namespace MultilingualChat.Server.Services;

public interface ITranslationService
{
   Task<string> TranslateAsync(string message, string sourceLanguage, string targetLanguage);
}