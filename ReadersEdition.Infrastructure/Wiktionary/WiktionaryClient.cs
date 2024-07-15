using System.Net.Http;
using ReadersEdition.Domain.DictionaryModels;

public class WiktionaryClient : HttpClient, IDictionaryRetriever
{
    public Task<Definition> GetDefinition(string word, Language wordLanguage, Language glossLanguage)
    {
        throw new NotImplementedException();
    }

    public Task<IDictionary<string, Definition>> GetDefinitions(IEnumerable<string> words, Language wordLanguage, Language glossLanguage)
    {
        throw new NotImplementedException();
    }
}