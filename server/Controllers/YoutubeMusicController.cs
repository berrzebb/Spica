using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

[ApiController]
[Route("[controller]")]
public class YoutubeMusicController : ControllerBase {
    private readonly IYoutubeMusicService _service;
    private readonly IMemoryCache _cache;
    private readonly ILogger _logger;
    public YoutubeMusicController(ILogger<YoutubeMusicController> logger, IYoutubeMusicService service, IMemoryCache cache) {
        _logger = logger;
        _service = service;
        _cache = cache;
    }
    [HttpGet]
    [Route("suggestions/{query}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSearchSuggestions(string query) {
        if(_cache.TryGetValue(query, out string cachedContent)){
            _logger.LogInformation($"[{HttpContext.Request.Path}] Cached");
            return Content(cachedContent, "application/json");
        } else {
        var result = await _service.getSearchSuggestions(query);
        _logger.LogInformation($"[{HttpContext.Request.Path}] {result.StatusCode}");

        if(result.IsSuccessStatusCode){
            var content = await result.Content.ReadAsStringAsync();
            var cachedEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(3));
            _cache.Set(query, content, cachedEntryOptions);
            return Content(content,"application/json");
        } else {
            return NotFound();
        }
        }
    }
    [HttpGet]
    [Route("search/{query}/{filter?}/{scope?}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Search(string query, string? filter = null, string? scope = null) {
        var searchedKey = Tuple.Create(query, filter, scope);
        if(_cache.TryGetValue(searchedKey, out string cachedContent)){
            _logger.LogInformation($"[{HttpContext.Request.Path}] Cached");
            return Content(cachedContent, "application/json");
        } else {
            var result = await _service.Search(query, filter, scope);
            _logger.LogInformation($"[{HttpContext.Request.Path}] {result.StatusCode}");

            if(result.IsSuccessStatusCode){
                var content = await result.Content.ReadAsStringAsync();
                var cachedEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(3));
                _cache.Set(searchedKey, content, cachedEntryOptions);
                return Content(content,"application/json");
            } else {
                return NotFound();
            }
        }

    }
        [HttpGet]
    [Route("search/{query}/{filter?}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Search(string query, string? filter = null) {
        return await Search(query, filter, "library");
    }
    [HttpGet]
    [Route("search/{query}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Search(string query) {
        return await Search(query, null, null);
    }
}