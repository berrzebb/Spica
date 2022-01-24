using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
[ApiController]
[Route("[controller]")]
public class YoutubeMusicController : Controller {
    private readonly IYoutubeMusicService _service;
    public YoutubeMusicController(IYoutubeMusicService service) {
        _service = service;
    }
    [HttpGet("suggestions/{query}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSearchSuggestions(string query) {
        var result = await _service.getSearchSuggestions(query);
        if(result.StatusCode == System.Net.HttpStatusCode.OK){
            return Json(await result.Content.ReadAsStringAsync());
        } else {
            return NotFound();
        }
    }
    [HttpGet("search/{query}/{filter?}/{scope?}")]
    [HttpGet("search/{query}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Search(string query, string? filter = "", string? scope = "") {
        var result = await _service.Search(query, filter, scope);
        Console.WriteLine($"[Search] {result.ToString()}");
        if(result.StatusCode == System.Net.HttpStatusCode.OK){
            return Json(await result.Content.ReadAsStringAsync());
        } else {
            return NotFound();
        }
    }
}