using MediatR;
using ReadersEdition.Domain.DictionaryModels;
using ReadersEdition.Domain;

public class GetDefinitionsQuery : IRequest<GetDefinitionsResult>
{

}

public class GetDefinitionsResult 
{
    public List<DefinitionDto> Words {get; set;} = new();
}

public class DefinitionDto {
    public Definition Definition {get; set;}
    public string WordLanguage {get; set;}
    public string GlossLanguage {get; set;}
}

public class GetDefinitionsHandler(IUnitOfWork db, IMediator mediator) : IRequestHandler<GetDefinitionsQuery, GetDefinitionsResult>
{
    public async Task<GetDefinitionsResult> Handle(GetDefinitionsQuery request, CancellationToken cancellationToken)
    {
        var languages = await db.GetLanguages();
        var allWords = await db.GetDefinitions();
        var result = new GetDefinitionsResult();
        foreach(var word in allWords)
        {
            var dto = new DefinitionDto();
            dto.Definition = word; 
            dto.WordLanguage = languages.Where(x => x.LanguageId == word.WordLanguageId).FirstOrDefault()?.LanguageName;
            dto.GlossLanguage = languages.Where(x => x.LanguageId == word.GlossLanguageId).FirstOrDefault()?.LanguageName;
            result.Words.Add(dto);
        }

        return result;
    }
}