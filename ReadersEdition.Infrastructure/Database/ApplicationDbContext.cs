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
        await SaveChangesAsync();
        return Result.Success();
    }
    public async Task<IEnumerable<Definition>> GetDefinitions()
    {
        return await Definitions.ToListAsync();
    }
    public async Task<IDictionary<string,Definition>> GetDefinitions(Document document, Language documentLanguage, Language glossLanguage)
    {
        var result = await Definitions.Where(x => document.Glosses.Keys.Any(y => y == x.Word && 
        x.GlossLanguageId == glossLanguage.LanguageId &&
        x.WordLanguageId == documentLanguage.LanguageId)).ToDictionaryAsync(x => x.Word, x => x);
        if(result == null)
            return new Dictionary<string,Definition>();
        foreach(var def in Definitions)
            Console.WriteLine(def.Word);
        
        return result;
    }

    public async Task<IEnumerable<Language>> GetLanguages()
    {
        return await Languages.ToListAsync();
    }

    public async Task<Result> AddLanguage(Language lang)
    {
        await Languages.AddAsync(lang);

        await SaveChangesAsync();

        return Result.Success();
    }
}