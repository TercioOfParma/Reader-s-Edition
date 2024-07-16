using MediatR;
using ReadersEdition.Domain.DictionaryModels;

public class GetLanguagesQuery : IRequest<GetLanguagesResult>
{

}

public class GetLanguagesResult 
{
    public IEnumerable<Language> Languages {get; set;}
}

public class GetLanguagesHandler : IRequestHandler<GetLanguagesQuery, GetLanguagesResult>
{
    public GetLanguagesHandler(IUnitOfWork db)
    {
        _db = db;
    }

    public IUnitOfWork _db { get; }

    public async Task<GetLanguagesResult> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
    {
        var result = await _db.GetLanguages();

        return new GetLanguagesResult { Languages = result};
    }
}