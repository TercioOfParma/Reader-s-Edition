using System.Globalization;
using System.Net.Http;
using System.Text;
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
        if(json != null && json[wordLanguage.LanguageCode] != null)
        {
            var currentLanguage = json[wordLanguage.LanguageCode].ToString();
            
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
        }
        if(word.ToLower() != word)
        {
            var result = await LoadWiktionaryDefinitions(word.ToLower(), wordLanguage, glossLanguage);
            definitions.AddRange(result);
        }
        return definitions;
    }
    private static string RemoveDiacritics(string text) 
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

        for (int i = 0; i < normalizedString.Length; i++)
        {
            char c = normalizedString[i];
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder
            .ToString()
            .Normalize(NormalizationForm.FormC);
    }
    /// <summary>
    /// Loads up all necessary definitions from the Wiktionary API
    /// </summary>
    /// <param name="words">List of Words Needed</param>
    /// <param name="wordLanguage">The Language of the Word</param>
    /// <param name="glossLanguage">The Language of the Gloss</param>
    /// <returns></returns>
    public async Task<IDictionary<string, List<Definition>>> GetDefinitions(IEnumerable<string> words, Language wordLanguage, Language glossLanguage, int depth = 0)
    {
        var dict = new Dictionary<string, List<Definition>>();
        int j = depth + 1;
        if(j == 10) //I've realised that there are infinite loops of links
            return dict;
        foreach(var word in words)
        {
            var cleanWord = RemoveDiacritics(word);
            var definitions = await LoadWiktionaryDefinitions(cleanWord, wordLanguage, glossLanguage);
            var inflections = definitions.Where(x => x.Gloss.Contains("form-of-definition-link") || x.Gloss.Contains("form-of-definition use-with-mention")).ToList();
            definitions.ToList().RemoveAll(x => inflections.Any(y => y.Gloss == x.Gloss));
            if(inflections.Count() != 0)
            {
                Console.WriteLine(cleanWord);
                var rawInflections = await StripHTMLFromInflections(inflections);
                var defined = await GetDefinitions(rawInflections.Keys, wordLanguage, glossLanguage, j);
                foreach(var list in defined.Values)
                {
                    list.ForEach(x => x.Word = word);
                    if(dict.ContainsKey(cleanWord))
                        dict[cleanWord].AddRange(list.ToList());
                    else 
                        dict[cleanWord] = list;
                }
            }
            StripHTMLFromDefinitions(definitions);
            var toAdd = definitions.ToList();
            toAdd.ForEach(x => x.Word = word);
            if(dict.ContainsKey(cleanWord))
                dict[cleanWord].AddRange(toAdd);
            else
                dict[cleanWord] = toAdd;
        }

        return dict;
    }
    private void StripHTMLFromDefinitions(IEnumerable<Definition> definitions)
    {
        var doc = new HtmlDocument();
        foreach(var def in definitions)
        {
            doc.LoadHtml(def.Gloss);
            var docContents = doc.DocumentNode;
            def.Gloss = docContents.InnerText;
           
        }
    }
    private async Task<IDictionary<string,string>> StripHTMLFromInflections(IEnumerable<Definition> definitions)
    {
        var strippedStrings = new Dictionary<string,string>();
        var doc = new HtmlDocument();
        foreach(var inflection in definitions)
        {
            doc.LoadHtml(inflection.Gloss);
            var docContents = doc.DocumentNode.SelectSingleNode("//span");
            var words = docContents.Descendants("span").Where(span => span.GetAttributeValue("class","").Contains("form-of-definition-link"));
            foreach(var word in words)
            {
                strippedStrings[word.InnerText] = inflection.Word;
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