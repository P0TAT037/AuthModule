using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Sample.Controllers;

[ApiController]
[Route("/")]
[Authorize]
public class TestController : ControllerBase
{
    private readonly ApplicationPartManager _partManager;

    public TestController(ApplicationPartManager partManager)
    {
        _partManager = partManager;
    }

    [HttpGet]
    public IActionResult GetFeatures()
    {
        return Ok(_partManager.FeatureProviders.Select(x=>x.ToString()).ToList());
    }
}