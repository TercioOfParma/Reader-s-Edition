using ReadersEdition.Domain.DictionaryModels;

public interface IDictionaryRetriever
{
    public Task<Definition> GetDefinition(string word, Language wordLanguage, Language glossLanguage); 
}