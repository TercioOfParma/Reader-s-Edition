using MediatR;
using ReadersEdition.Domain.DictionaryModels;
using ReadersEdition.Domain;

public class GetDefinitionsForTextQuery : IRequest<GetDefinitionsForTextResult>
{
    public Language TextLanguage {get; set;}
    public Language GlossLanguage {get; set;}
    public string Text {get; set;}
    public bool ComprehensibleInput {get; set;}
    public int ComprehensibleInputThreshold {get; set;}
    public int FrequencyInFileThreshold {get; set;}
}

public class GetDefinitionsForTextResult
{

}

public class GetDefinitionsForTextHandler : IRequestHandler<GetDefinitionsForTextQuery, GetDefinitionsForTextResult>
{
    public async Task<GetDefinitionsForTextResult> Handle(GetDefinitionsForTextQuery request, CancellationToken cancellationToken)
    {
        var result = new GetDefinitionsForTextResult();

        return result;
    }
}