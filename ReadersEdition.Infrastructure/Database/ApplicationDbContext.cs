using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using ReadersEdition.Domain.DictionaryModels;


public class ApplicationDbContext : DbContext, IUnitOfWork
{
    private DbSet<Definition> Definitions {get; set;}
    private DbSet<Language> Languages {get; set;}
    public Task<Result> AddDefinitions(IEnumerable<Definition> Words, Language wordLanguage, Language glossLanguage)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Definition>> GetDefinitions(Document document, Language documentLanguage, Language glossLanguage)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Language>> GetLanguages()
    {
        throw new NotImplementedException();
    }
}