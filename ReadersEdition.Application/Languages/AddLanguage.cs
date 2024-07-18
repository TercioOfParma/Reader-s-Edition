
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
        await _db.AddLanguage(request.Lang);
        return Result.Success();
    }
}