using MediatR;
using ReadersEdition.Domain.DictionaryModels;
using ReadersEdition.Domain;
using System.Text;

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
    public List<WordInstance> WordInstances {get; set;} = new();
}

    public class WordInstance{
        public string Word {get; set;} = string.Empty;
        public string SurroundingSentence {get; set;} = string.Empty;
        public List<string> DisplayDefinitions {get; set;} = new();
        public List<string> AllDefinitions {get; set;} = new();
    }

public class GetDefinitionsForTextHandler(IUnitOfWork _db) : IRequestHandler<GetDefinitionsForTextQuery, GetDefinitionsForTextResult>
{
    public async Task<GetDefinitionsForTextResult> Handle(GetDefinitionsForTextQuery request, CancellationToken cancellationToken)
    {
        var result = new GetDefinitionsForTextResult();
        var words = StripVocabulary(request.Text).ToList();
        words.RemoveAll(x => x == string.Empty);
        var i = 0;
        if(!request.ComprehensibleInput)
        {
            var frequencyWords = GetWordsForFrequency(words.ToList(), request.FrequencyInFileThreshold);
            var definitions = await _db.GetDefinitions(frequencyWords, request.TextLanguage, request.GlossLanguage);
            
            foreach(var word in words)
            {
                var instance = new WordInstance { Word = word};
                var unmodifiedDefs = definitions.Where(x => x.Word == word || x.Word == word.ToLower()).Select(x => x.Gloss).ToList();
                unmodifiedDefs.RemoveAll(x => x == string.Empty);
                instance.AllDefinitions = unmodifiedDefs.Distinct().ToList();
                instance.SurroundingSentence = GenerateSentenceContext(words, i);
                i++;
                
                result.WordInstances.Add(instance);
            }
        }
        else 
        {
            var frequencyWords = await GetWordsForComprehensibleInput(words.ToList(), request.ComprehensibleInputThreshold, request.TextLanguage, request.GlossLanguage);
            foreach(var word in words)
            {
                var instance = new WordInstance { Word = word};
                var unmodifiedDefs = frequencyWords.Where(x => x.Word == word || x.Word == word.ToLower()).Select(x => x.Gloss).ToList();
                unmodifiedDefs.RemoveAll(x => x == string.Empty);
                instance.AllDefinitions = unmodifiedDefs.Distinct().ToList();
                instance.SurroundingSentence = GenerateSentenceContext(words, i);
                i++;
                result.WordInstances.Add(instance);
            }
        }
        return result;
    }
    public string[] StripVocabulary(string file)
    {
        var charArray = file.ToArray();
        var builder = new StringBuilder();
        foreach(var character in charArray)
        {
            if(character == '\n' || character == '\r' || character == '\t')
                builder.Append(' ');
            else if(!char.IsPunctuation(character))
            {
                builder.Append(character);
            }
        }
        return builder.ToString().Split(" ");
    }
    public async Task<List<Definition>> GetWordsForComprehensibleInput(List<string> words, int threshold, Language textLanguage, Language glossLanguage)
    {
        var relevantWords = await _db.GetComprehensibleInputDefinitions(words, threshold,textLanguage,glossLanguage);
        return relevantWords.ToList();
    }
    public string GenerateSentenceContext(List<string> words, int position)
    {
        var i = 0;
        var context = "";
        if(position < 4)
        {
            for(i = 1; i < 8; i++)
                context = context + words[i - 1] + " ";
        }
        else if(position >= words.Count() - 4)
        {
            for(i = words.Count(); i > words.Count() - 5; i--)
                context = context +  words[i - 1] + " ";
        }
        else 
        {
            for(i = position - 2; i < position + 4; i++)
                context = context + words[i - 1] + " ";
        }
        return context;
    }
    public List<string> GetWordsForFrequency(List<string> words, int threshold)
    {
        var relevantWords = new Dictionary<string,int>();
        var toStrip = words.ToList();
        var toSearchUp = toStrip.Distinct().ToList();
        while(toStrip.Count() != 0)
        {
            var word = toStrip.FirstOrDefault();
            var count = toStrip.Where(x => x == word).Count();
            if(count <= threshold)
                relevantWords[word] = count;
            toStrip.RemoveAll(x => x == word);

        }

        return relevantWords.Keys.ToList();
    }
}