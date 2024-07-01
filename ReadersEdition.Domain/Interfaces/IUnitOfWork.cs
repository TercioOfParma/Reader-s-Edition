using System.Reflection.Metadata;
using System.Xml.XPath;
using ReadersEdition.Domain.DictionaryModels;

public interface IUnitOfWork
{
    public Task<IEnumerable<Language>> GetLanguages();
    public Task<IEnumerable<Definition>> GetDefinitions(Document document, Language documentLanguage, Language glossLanguage);
    public Task<Result> AddDefinitions(IEnumerable<Definition> Words, Language wordLanguage, Language glossLanguage);
}