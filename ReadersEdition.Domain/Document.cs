using System.Text;

namespace ReadersEdition.Domain;
/**
<summary>
Class <c>Document</c> is the main class which all document information is stored and all universal 
operations carried out
</summary>

<param name="GlossedAsComprehensibleInput">Determines if it will be gloss words based on overall frequency for all words in language</param>
<param name="GlossedAsBible">Determines if it will be gloss words based on occurences within the text itself</param>
<param name="Threshold">This threshold detemines whether a word should be glossed or not. If GlossedAsBible, this is the number of occurences and chooses those lower. 
If GlossedAsComprehensibleInput, it's this value or higher</param>
<param name="DocumentContents">The contents of the file</param>
**/
public class Document
{
    public bool GlossedAsComprehensibleInput {get; set;}
    public bool GlossedAsBible => !GlossedAsComprehensibleInput;
    public int Threshold {get; set;}
    public string DocumentContents {get; set;}
    public Dictionary<string, string> Glosses {get; set;} = new();
    public Dictionary<string, int> FrequencyInDocument {get; set;} = new();
    /**
        <summary> Constructor </summary>
        <param name="fileContents"> The text contents of the file </param>
        <param name="comprehensibleInput"> Whether or not it is glossed according to Comprehensible Input Rules </param>
        <param name="threshold"> The threshold for glossing </param>
    **/
    public Document(string fileContents, bool comprehensibleInput)
    {
        GlossedAsComprehensibleInput = comprehensibleInput;
        DocumentContents = fileContents;
        ContentsToGloss();
    }
    /// <summary>
    /// Creates a new string which is the file with all punctuation stripped and returns it
    /// </summary>
    /// <returns>A new string with all punctuation stripped</returns>
    private string StripPunctuation()
    {
        var str = new StringBuilder();

        foreach(char c in DocumentContents)
        {
            if(char.IsPunctuation(c))
                str.Append(' ');
            else 
                str.Append(c);
        }
        return str.ToString();
    }
    /// <summary>
    /// Converts Content of File to a Base Glossing Dictionary, and if applicable, a Frequency Dictionary as well
    /// </summary>
    private void ContentsToGloss()
    {
        var strippedContent = StripPunctuation();
        var toConvert = strippedContent.Split(" ").Distinct();
        Glosses = toConvert.ToDictionary(x => x);
        if(GlossedAsBible)
            GenerateFrequencies(strippedContent);
    }
    /// <summary>
    /// Generates Frequency Dictionary
    /// </summary>
    /// <param name="content">The stripped content of the file</param>
    private void GenerateFrequencies(string content)
    {
        var contents = content.Split(" ");
        foreach(var word in contents)
        {
            if(FrequencyInDocument.Any(x => x.Key == word))
                FrequencyInDocument[word] += 1;
            else 
                FrequencyInDocument[word] = 1;
        }
    }
    /// <summary>
    /// Returns the Proper Gloss Dictionary For Getting Loading In Definitions
    /// </summary>
    /// <returns>A Dictionary with the Proper Words for Glossing</returns>
    public Dictionary<string, string> GetRelevantGlosses(IDictionaryRetriever retriever, IUnitOfWork _db)
    {
        var relevantGlosses = new Dictionary<string, string>();
        foreach(var pair in FrequencyInDocument)
        {
            if((GlossedAsBible && pair.Value <= Threshold) || (GlossedAsComprehensibleInput && pair.Value >= Threshold))
            {
                relevantGlosses[pair.Key] = "";    
            }           
        }

        return relevantGlosses;
    }
}
