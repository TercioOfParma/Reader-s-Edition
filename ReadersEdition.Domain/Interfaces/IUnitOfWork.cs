using System.Xml.XPath;
using ReadersEdition.Domain.DictionaryModels;
using ReadersEdition.Domain;

public interface IUnitOfWork
{
    public Task<IEnumerable<Language>> GetLanguages();
    public Task<IDictionary<string,Definition>> GetDefinitions(Document document, Language documentLanguage, Language glossLanguage);
    public Task<Result> AddDefinitions(IEnumerable<Definition> Words, Language wordLanguage, Language glossLanguage);
}