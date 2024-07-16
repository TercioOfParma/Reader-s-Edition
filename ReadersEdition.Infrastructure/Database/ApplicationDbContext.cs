using Microsoft.EntityFrameworkCore;
using ReadersEdition.Domain.DictionaryModels;
using ReadersEdition.Domain;


public class ApplicationDbContext : DbContext, IUnitOfWork
{
    private DbSet<Definition> Definitions {get; set;}
    private DbSet<Language> Languages {get; set;}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=my_db;Username=my_user;Password=my_pw");
    public async Task<Result> AddDefinitions(IEnumerable<Definition> Words, Language wordLanguage, Language glossLanguage)
    {
        await Definitions.AddRangeAsync(Words);
        return Result.Success();
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