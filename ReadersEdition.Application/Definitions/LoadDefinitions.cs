using MediatR;
using ReadersEdition.Domain.DictionaryModels;
using ReadersEdition.Domain;
using System.Security.Cryptography.X509Certificates;

public class LoadDefinitionsQuery : IRequest<LoadDefinitionsResult>
{
    public Language TextLanguage {get; set;}
    public Language GlossLanguage {get; set;}
    public string Text {get; set;}
    public bool ComprehensibleInput {get; set;}
}

public class LoadDefinitionsResult 
{
    public IDictionary<string, Definition> Definitions {get; set;}
}

public class LoadDefinitionsHandler : IRequestHandler<LoadDefinitionsQuery, LoadDefinitionsResult>
{
    private IUnitOfWork _db {get; set;}
    private IDictionaryRetriever _retriever {get; set;}
    public LoadDefinitionsHandler(IUnitOfWork db, IDictionaryRetriever retriever)
    {
        _db = db;
        _retriever = retriever;
    }
    public async Task<LoadDefinitionsResult> Handle(LoadDefinitionsQuery request, CancellationToken cancellationToken)
    {
        var document = new Document(request.Text, request.ComprehensibleInput);
        var dbDefinitions = await _db.GetDefinitions(document, request.TextLanguage, request.GlossLanguage);
        var missingDefinitions = document.Glosses.Keys.Where(x => dbDefinitions.Keys.Any(y => y == x)).ToList();
        var wiktionaryDefinitions = await _retriever.GetDefinitions(missingDefinitions, request.TextLanguage, request.GlossLanguage);
        await _db.AddDefinitions(wiktionaryDefinitions.Values, request.TextLanguage, request.GlossLanguage);
        foreach(var pair in wiktionaryDefinitions)
            dbDefinitions.Add(pair);
        return new LoadDefinitionsResult{ Definitions = dbDefinitions};
    }
}