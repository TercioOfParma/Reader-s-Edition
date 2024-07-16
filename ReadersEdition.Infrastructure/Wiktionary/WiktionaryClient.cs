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
    /// <summary>
    /// Loads in the Definitions directly prior to preprocessing
    /// </summary>
    /// <param name="word">The word to load</param>
    /// <param name="wordLanguage">The language of the Word</param>
    /// <param name="glossLanguage">The language of the Gloss</param>
    /// <returns></returns>
    public async Task<IEnumerable<Definition>> LoadWiktionaryDefinitions(string word, Language wordLanguage, Language glossLanguage)
    {
        var definitions = new List<Definition>();
        //form-of-definition-link is very important, as this indicates where re-searching should occur
        var textInfo = await GetAsync(_baseAddress + word);
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
                newDefinition.GlossLanguageId = glossLanguage.LanguageId;
                newDefinition.WordLanguageId = wordLanguage.LanguageId;
                definitions.Add(newDefinition);
            }
        }
        return definitions;
    }
    /// <summary>
    /// Loads up all necessary definitions from the Wiktionary API
    /// </summary>
    /// <param name="words">List of Words Needed</param>
    /// <param name="wordLanguage">The Language of the Word</param>
    /// <param name="glossLanguage">The Language of the Gloss</param>
    /// <returns></returns>
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
                var rawInflections = await StripHTMLFromInflections(inflections);
                var defined = await GetDefinitions(rawInflections, wordLanguage, glossLanguage);
                defined.ToList().ForEach(x => dict.Add(x.Key, x.Value));
            }
            StripHTMLFromDefinitions(definitions);
            foreach(var definition in definitions)
            {
                Console.WriteLine($"{definition.Word}");
                dict.Add(definition.Word, definition);
            }
        }

        return dict;
    }
    private void StripHTMLFromDefinitions(IEnumerable<Definition> definitions)
    {
        var doc = new HtmlDocument();
        foreach(var def in definitions)
        {
            doc.LoadHtml(def.Gloss);
            var docContents = doc.DocumentNode.SelectSingleNode("//span");
            var words = docContents.Descendants("span").Where(span => span.GetAttributeValue("class","") == "form-of-definition-link");
            def.Gloss = "";
            foreach(var word in words)
            {
                def.Gloss += word;
            }
        }
    }
    private async Task<IEnumerable<string>> StripHTMLFromInflections(IEnumerable<Definition> definitions)
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

    public void ChangeLanguage(Language glossLanguage)
    {
        _baseAddress = $"https://{glossLanguage.LanguageCode}.wiktionary.org/api/rest_v1/page/definition/";
    }
}