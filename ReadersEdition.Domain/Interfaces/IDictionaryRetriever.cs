using ReadersEdition.Domain.DictionaryModels;

public interface IDictionaryRetriever
{
    public void ChangeLanguage(Language glossLanguage);
    public Task<Definition> GetDefinition(string word, Language wordLanguage, Language glossLanguage); 
    public Task<IDictionary<string, List<Definition>>> GetDefinitions(IEnumerable<string> words, Language wordLanguage, Language glossLanguage, int i = 0);
}