namespace MultilingualChat.Server.Services;

public enum LargeLanguageModelName
{
    Llama3_8b,
    Mixtral_8x7b,
    Gemma_7b
}
public class LargeLanguageModelConfig
{
    public Dictionary<LargeLanguageModelName, string> Models { get; set; }
}