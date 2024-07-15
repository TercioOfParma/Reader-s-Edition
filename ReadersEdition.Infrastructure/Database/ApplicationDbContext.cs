using Microsoft.EntityFrameworkCore;
using ReadersEdition.Domain.DictionaryModels;
using ReadersEdition.Domain;


public class ApplicationDbContext : DbContext, IUnitOfWork
{
    private DbSet<Definition> Definitions {get; set;}
    private DbSet<Language> Languages {get; set;}
    public Task<Result> AddDefinitions(IEnumerable<Definition> Words, Language wordLanguage, Language glossLanguage)
    {
        throw new NotImplementedException();
    }

    public async Task<IDictionary<string,Definition>> GetDefinitions(Document document, Language documentLanguage, Language glossLanguage)
    {
        var result = await Definitions.Where(x => document.Glosses.Any(y => y.Key == x.Word)).ToDictionaryAsync(x => x.Word, x => x);
        if(result == null)
            return new Dictionary<string,Definition>();
        return result;
    }

    public Task<IEnumerable<Language>> GetLanguages()
    {
        throw new NotImplementedException();
    }
}