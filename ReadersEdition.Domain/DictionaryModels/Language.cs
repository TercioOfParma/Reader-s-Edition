namespace ReadersEdition.Domain.DictionaryModels;

/// <summary>
/// This contains information on a Language
/// </summary>
public class Language
{
    /// <summary>
    /// The Id of the Language 
    /// </summary>
    public Guid LanguageId {get; set;}
    /// <summary>
    /// The name of the language
    /// </summary>
    public string LanguageName {get; set;}
    /// <summary>
    /// The Code that the language has (This is used for Wiktionary but there's no reason it won't work elsewhere)
    /// </summary>
    public string LanguageCode {get; set;}

    public Language()
    {
        LanguageId = Guid.NewGuid();
    }    
}