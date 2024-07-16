using Newtonsoft.Json;

public class WiktionaryLanguageResponse {
    [JsonProperty("partOfSpeech")]
    public string PartOfSpeech{get; set;}
    [JsonProperty("Language")]
    public string Language {get; set;}
    [JsonProperty("definitions")]
    public IEnumerable<WiktionaryApiDefinition> Definitions {get; set;}

}