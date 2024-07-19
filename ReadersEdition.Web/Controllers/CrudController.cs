using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReadersEdition.Domain.DictionaryModels;
using ReadersEdition.Web.Models;

public class CrudController : Controller 
{
    private readonly ILogger<CrudController> _logger;
    private IMediator _mediator {get; set;}

    public CrudController(ILogger<CrudController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> CreateLanguage()
    {
        return View(new NewLanguageDto());
    }
    [HttpPost]
    public async Task<IActionResult> CreateLanguage(NewLanguageDto model)
    {
        var language = new Language { LanguageCode = model.LanguageCode, LanguageName = model.LanguageName};
        await _mediator.Send(new AddLanguageCommand{ Lang = language });
        return RedirectToAction("Index", "Home");
    }
}