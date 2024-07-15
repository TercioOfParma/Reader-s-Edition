

using ReadersEdition.Domain.DictionaryModels;

public class WiktionaryClient : IDictionaryRetriever
{
    public Task<Definition> GetDefinition(string word, Language wordLanguage, Language glossLanguage)
    {
        throw new NotImplementedException();
    }
}