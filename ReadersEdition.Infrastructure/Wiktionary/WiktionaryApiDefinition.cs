using Newtonsoft.Json;

public class WiktionaryApiDefinition 
{
    [JsonProperty("definition")]
    public string Definition {get; set;}
}