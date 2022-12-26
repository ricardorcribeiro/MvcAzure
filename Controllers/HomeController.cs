using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using MvcAzure.Models;

namespace MvcAzure.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly GraphServiceClient _graphServiceClient;

    public HomeController(ILogger<HomeController> logger,
                      GraphServiceClient graphServiceClient)
    {
        _logger = logger;
        _graphServiceClient = graphServiceClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var teste = Request.Headers;
        var user = await _graphServiceClient.Me.Request().GetAsync();
        ViewData["ApiResult"] = user.DisplayName;
        return View();
    }

    [HttpPost]
    public async void post([FromBody] Pessoa pessoa)
    {
        var teste = Request.Headers;
        var user = await _graphServiceClient.Me.Request().GetAsync();
    }

    [Authorize]
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
public class Pessoa
{
    public string Id_Token { get; set; }
}