using ReadersEdition.Domain;

namespace ReadersEdition.Tests;

public class DocumentTests
{
    public Document Doc {get; set;}
    [SetUp]
    public void Setup()
    {
        Doc = new Document("This*is,A.Proper!Test;For Vocab", true);
    }

    [Test]
    public void ProducesCorrectGlossSize()
    {
        if(Doc.Glosses.Count == 7)
            Assert.Pass();
        Assert.Fail();
    }
    [Test]
    public void ProducesCorrectFrequencyDictionarySize()
    {
        if(Doc.FrequencyInDocument.Count == 7)
            Assert.Pass();
        Assert.Fail();
    }
}