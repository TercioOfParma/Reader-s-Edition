
using ReadersEdition.Domain.DictionaryModels;
using MediatR;

public class AddLanguageCommand : IRequest<Result>
{
    public Language Lang{get; set;}
}

public class AddLanguageHandler : IRequestHandler<AddLanguageCommand, Result>
{
    private readonly IUnitOfWork _db;

    public AddLanguageHandler(IUnitOfWork db)
    {
        _db = db;
    }
    public async Task<Result> Handle(AddLanguageCommand request, CancellationToken cancellationToken)
    {
        var languages = await _db.GetLanguages();

        if(languages.Any(x => x.LanguageCode == request.Lang.LanguageCode || 
        x.LanguageName == request.Lang.LanguageName))
            return Result.Error("Language Already Exists!");
        await _db.AddLanguage(request.Lang);
        return Result.Success();
    }
}