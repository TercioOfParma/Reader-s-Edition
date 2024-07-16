using System.Net.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReadersEdition.Domain.DictionaryModels;

public class WiktionaryClient : HttpClient, IDictionaryRetriever
{
    private string _baseAddress {get; set;}
    public WiktionaryClient(string baseAddress)
    {
        _baseAddress = baseAddress;
    }
    public async Task<IEnumerable<Definition>> LoadWiktionaryDefinitions(string word, Language wordLanguage, Language glossLanguage)
    {
        var definitions = new List<Definition>();
        //form-of-definition-link is very important, as this indicates where re-searching should occur
        var textInfo = await GetAsync(_baseAddress + $"/rest_v1/page/definition/{word}");
        var message = await textInfo.Content.ReadAsStringAsync();
        var json = JObject.Parse(message);
        var currentLanguage = json[glossLanguage.LanguageCode].ToString();
        
        var fullResponse = JsonConvert.DeserializeObject<IEnumerable<WiktionaryLanguageResponse>>(currentLanguage);
        foreach(var response in fullResponse)
        {
            foreach(var definition in response.Definitions)
            {
                var newDefinition = new Definition();
                newDefinition.DefinitionId = Guid.NewGuid();
                newDefinition.Word = word;
                newDefinition.Gloss = definition.Definition;
                definitions.Add(newDefinition);
            }
        }
        return definitions;
    }

    public async Task<IDictionary<string, Definition>> GetDefinitions(IEnumerable<string> words, Language wordLanguage, Language glossLanguage)
    {
        var dict = new Dictionary<string, Definition>();
        foreach(var word in words)
        {
            var definitions = await LoadWiktionaryDefinitions(word, wordLanguage, glossLanguage);
            var inflections = definitions.Where(x => x.Gloss.Contains("form-of-definition-link"));
            definitions.ToList().RemoveAll(x => inflections.Any(y => y.Gloss == x.Gloss));
            if(inflections.Count() != 0)
            {
                var rawInflections = await StripHTMLFromDefinitions(inflections);
                var defined = await GetDefinitions(rawInflections, wordLanguage, glossLanguage);
                defined.ToList().ForEach(x => dict.Add(x.Key, x.Value));
            }
            foreach(var definition in definitions)
            {
                Console.WriteLine($"{definition.Word}");
                dict.Add(definition.Word, definition);
            }
        }

        return dict;
    }
    private async Task<IEnumerable<string>> StripHTMLFromDefinitions(IEnumerable<Definition> definitions)
    {
        var strippedStrings = new List<string>();
        var doc = new HtmlDocument();
        foreach(var inflection in definitions)
        {
            doc.LoadHtml(inflection.Gloss);
            var docContents = doc.DocumentNode.SelectSingleNode("//span");
            var words = docContents.Descendants("span").Where(span => span.GetAttributeValue("class","") == "form-of-definition-link");
            foreach(var word in words)
            {
                strippedStrings.Add(word.InnerText);
            }
        }
        return strippedStrings;
    }

    public Task<Definition> GetDefinition(string word, Language wordLanguage, Language glossLanguage)
    {
        throw new NotImplementedException();
    }
}