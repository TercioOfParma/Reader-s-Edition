using Microsoft.EntityFrameworkCore;
using ReadersEdition.Domain.DictionaryModels;
using ReadersEdition.Domain;


public class ApplicationDbContext : DbContext, IUnitOfWork
{
    private DbSet<Definition> Definitions {get; set;}
    private DbSet<Language> Languages {get; set;}
   private string DbPath {get; set;}
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    public ApplicationDbContext()
    {
        
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "ReadersEdition.db");
    }
    public async Task<IEnumerable<Definition>> GetComprehensibleInputDefinitions(List<string> words, int threshold, Language textLanguage, Language glossLanguage)
    {
        return await Definitions.Where(x => words.Contains(x.Word)).ToListAsync();
    }
    public async Task<Result> AddDefinitions(IEnumerable<Definition> Words, Language wordLanguage, Language glossLanguage)
    {
        await Definitions.AddRangeAsync(Words);
        await SaveChangesAsync();
        return Result.Success();
    }
    public async Task<IEnumerable<Definition>> GetDefinitions(List<string> words, Language textLanguage, Language glossLanguage)
    {
        var lowerCase = new List<string>();
        words.ForEach(x => lowerCase.Add(x.ToLower()));
        return await Definitions.Where(x => (words.Contains(x.Word) || lowerCase.Contains(x.Word)) &&
        x.GlossLanguageId == glossLanguage.LanguageId && x.WordLanguageId == textLanguage.LanguageId
        ).ToListAsync();
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