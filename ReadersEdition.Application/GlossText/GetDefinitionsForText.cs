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

public class GetDefinitionsForTextHandler(IUnitOfWork _db) : IRequestHandler<GetDefinitionsForTextQuery, GetDefinitionsForTextResult>
{
    public async Task<GetDefinitionsForTextResult> Handle(GetDefinitionsForTextQuery request, CancellationToken cancellationToken)
    {
        var result = new GetDefinitionsForTextResult();
        var words = request.Text.Split(" ");
        if(!request.ComprehensibleInput)
        {
            var frequencyWords = GetWordsForFrequency(words.ToList(), request.FrequencyInFileThreshold);
            var definitions = await _db.GetDefinitions(frequencyWords);
        }
        else 
        {
            var frequencyWords = GetWordsForComprehensibleInput(words.ToList(), request.ComprehensibleInputThreshold);
        }
        return result;
    }
    public List<string> GetWordsForComprehensibleInput(List<string> words, int threshold)
    {
        var relevantWords = words.ToList();


        return relevantWords;
    }
    public List<string> GetWordsForFrequency(List<string> words, int threshold)
    {
        var relevantWords = new Dictionary<string,int>();
        var toStrip = words.ToList();
        while(toStrip.Count() != 0)
        {
            var word = toStrip.FirstOrDefault();
            var count = toStrip.Where(x => x == word).Count();
            if(count <= threshold)
                relevantWords[word] = count;

        }

        return relevantWords.Keys.ToList();
    }
}