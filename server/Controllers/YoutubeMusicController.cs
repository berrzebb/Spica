using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("[controller]")]
public class YoutubeMusicController : ControllerBase {
    private readonly IYoutubeMusicService _service;
    public YoutubeMusicController(IYoutubeMusicService service) {
        _service = service;
    }
    [HttpGet("suggestions/{query}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> GetSearchSuggestions(string query) {
        var result = await _service.getSearchSuggestions(query);
        if(result.StatusCode == System.Net.HttpStatusCode.OK){
            return await result.Content.ReadAsStringAsync();
        } else {
            return NotFound();
        }
    }
    [HttpGet("search/{query}/{filter=songs}/{scope=library}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> Search(string query, string filter = "songs", string scope = "library") {
        var result = await _service.Search(query, filter, scope);
        if(result.StatusCode == System.Net.HttpStatusCode.OK){
            return await result.Content.ReadAsStringAsync();
        } else {
            return NotFound();
        }
    }
}