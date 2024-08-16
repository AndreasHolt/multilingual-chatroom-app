using System.Collections.Generic;

namespace MultilingualChat.Client.ViewModels;

public class Language
{
   public Language(string languageName)
   {
      LanguageName = languageName;
   }
   public string LanguageName { get; set; }
}