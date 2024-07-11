namespace ReadersEdition.Web.Models;

public class GlossTextDto 
{
    public IFormFile ContentFile {get; set;}
    public bool GlossedAsComprehensibleInput {get; set;}
    public int Threshold {get; set;}
}