using ReadersEdition.Domain.DictionaryModels;

public interface IDictionaryRetriever
{
    public Task<Definition> GetDefinition(string word, Language wordLanguage, Language glossLanguage); 
    public Task<IDictionary<string, Definition>> GetDefinitions(IEnumerable<string> words, Language wordLanguage, Language glossLanguage);
}