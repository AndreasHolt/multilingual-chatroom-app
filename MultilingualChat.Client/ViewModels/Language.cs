namespace MultilingualChat.Client.Models;

public class Language
{
    public Language(string languageName)
    {
        LanguageName = languageName;
    }

    public string LanguageName { get; set; }

    public override string ToString()
    {
        return LanguageName;
    }
}