


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
    private string _preamble = "\\documentclass[11pt]{book}\\usepackage{atbegshi}\\usepackage[margin=1in]{geometry}\\usepackage{pdfcomment}\\usepackage{xcolor}\\usepackage{multicol}\\setcounter{secnumdepth}{-1}\\begin{document}\\sloppy\n";
    private string _conclusion = "\\end{document}";
    public GenerateLaTeXDocumentHandler()
    {
    }

    public async Task<GenerateLaTeXDocumentResult> Handle(GenerateLaTeXDocumentCommand request, CancellationToken cancellationToken)
    {
        var result = new GenerateLaTeXDocumentResult {};
        var strippedText = StripWord(request.Text);
        var splitWords = string.Join(" ", strippedText).Split(" ");
        var processedFile = _preamble;
        var definitions = request.GlossedWords.ToList();
        foreach(var word in splitWords)
        {
            var completeWord = String.Join("", StripWord(word));
            var def = definitions.FirstOrDefault();
            if(def == null)
                break;
            if(completeWord == def.Word)
            {
                processedFile += def.DisplayDefinitions.Count != 0 ? word + "\\footnote{" + String.Join(", ", def.DisplayDefinitions) + "} " : word + " ";
                definitions.Remove(def);
            }
            else 
                processedFile += word + " ";
        }
        processedFile +=_conclusion;
        result.GlossedText = processedFile;
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