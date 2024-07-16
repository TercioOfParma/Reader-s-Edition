using ReadersEdition.Domain.DictionaryModels;

namespace ReadersEdition.Web.Models;

public class GlossTextDto 
{
    public IFormFile ContentFile {get; set;}
    public bool GlossedAsComprehensibleInput {get; set;}
    public DifficultyLevels Threshold {get; set;}
    public Language GlossLanguage {get; set;}
    public Language DefinitionLanguage {get; set;}
    public IEnumerable<Language> Languages {get; set;}
}