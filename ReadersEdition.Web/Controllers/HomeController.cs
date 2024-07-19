using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReadersEdition.Web.Models;

namespace ReadersEdition.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IMediator _mediator {get; set;}

    public HomeController(ILogger<HomeController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GlossText()
    {
        var gloss = new GlossTextDto();
        var languages = (await _mediator.Send(new GetLanguagesQuery())).Languages;
        gloss.Languages = languages.ToList().ConvertAll(x => new SelectListItem(x.LanguageName, x.LanguageId.ToString()));
        return View(gloss);
    }
    [HttpPost]
    public async Task<IActionResult> GlossText(GlossTextDto glossText)
    {
        var resultStream = glossText.ContentFile.OpenReadStream();
        resultStream.Position = 0;
        var streamToReadFrom = new StreamReader(resultStream);
        var fileContents = await streamToReadFrom.ReadToEndAsync();
        var result = await _mediator.Send(new LoadDefinitionsQuery { Text = fileContents, GlossLanguage = glossText.GlossLanguage, TextLanguage = glossText.TextLanguage } );
        Console.WriteLine(fileContents);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
