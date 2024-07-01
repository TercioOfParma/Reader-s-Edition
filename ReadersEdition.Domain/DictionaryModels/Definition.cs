/// <summary>
/// This contains the information for a Definition of a word
/// </summary>
public class Definition
{
    /// <summary>
    /// The Id for the definition itself
    /// </summary>
    public Guid DefinitionId {get; set;}
    /// <summary>
    /// The ID for the Language that the Word is in
    /// </summary>
    public Guid WordLanguageId {get; set;}
    /// <summary>
    /// The ID for the Language that the Gloss is in
    /// </summary>
    public Guid GlossLanguageId {get;set;}
    /// <summary>
    /// The word itself
    /// </summary>
    public string Word {get; set;}
    /// <summary>
    /// The word's definition
    /// </summary>
    public string Gloss {get; set;}
}