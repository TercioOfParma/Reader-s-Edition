using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult GlossText()
    {
        return View(new GlossTextDto());
    }
    [HttpPost]
    public async Task<IActionResult> GlossText(GlossTextDto glossText)
    {
        var resultStream = glossText.ContentFile.OpenReadStream();
        resultStream.Position = 0;
        var streamToReadFrom = new StreamReader(resultStream);
        var fileContents = await streamToReadFrom.ReadToEndAsync();
        var result = await _mediator.Send(new LoadDefinitionsQuery { Text = fileContents, GlossLanguage = glossText.GlossLanguage, TextLanguage = glossText.DefinitionLanguage } );
        Console.WriteLine(fileContents);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
