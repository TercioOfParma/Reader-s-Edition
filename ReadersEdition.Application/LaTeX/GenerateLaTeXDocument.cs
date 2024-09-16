


using MediatR;

public class GenerateLaTeXDocumentCommand(string text, List<WordInstance> wordInstances) : IRequest<GenerateLaTeXDocumentResult>
{
    public string Text {get; set;} = text;
    public List<WordInstance> GlossedWords {get; set;} = wordInstances;
}

public class GenerateLaTeXDocumentResult
{
    public string GlossedText {get; set;}
}

public class GenerateLaTeXDocumentHandler : IRequestHandler<GenerateLaTeXDocumentCommand, GenerateLaTeXDocumentResult>
{
    public GenerateLaTeXDocumentHandler()
    {
    }

    public async Task<GenerateLaTeXDocumentResult> Handle(GenerateLaTeXDocumentCommand request, CancellationToken cancellationToken)
    {
        var result = new GenerateLaTeXDocumentResult {};
        var splitWords = request.Text.Split(" ");
        var processedFile = "";
        var definitions = request.GlossedWords.ToList();
        foreach(var word in splitWords)
        {
            var completeWord = String.Join("", StripWord(word));
            var def = definitions.FirstOrDefault();
            if(completeWord == def.Word)
            {
                processedFile += processedFile + word + "\\footnote{" + String.Join(", ", def.DisplayDefinitions) + "} ";
                definitions.Remove(def);
            }
            else 
                processedFile += processedFile + word + " ";
        }
        return result;
    }

        public string[] StripWord(string word)
    {
        var charArray = word.ToArray();
        var builder = new System.Text.StringBuilder();
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
}