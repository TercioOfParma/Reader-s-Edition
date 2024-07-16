using Microsoft.AspNetCore.Mvc.Rendering;
using ReadersEdition.Domain.DictionaryModels;

namespace ReadersEdition.Web.Models;

public class GlossTextDto 
{
    public IFormFile ContentFile {get; set;}
    public bool GlossedAsComprehensibleInput {get; set;}
    public DifficultyLevels Threshold {get; set;}
    public Language GlossLanguage {get; set;}
    public Language TextLanguage {get; set;}
    public IEnumerable<SelectListItem> Languages {get; set;}
}