using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
public class YoutubeMusicService : IYoutubeMusicService {
    HttpClientHandler _handler;
    HttpClient _client;
    ILogger _logger;
    YoutubeConfig _configs;
    public YoutubeMusicService(ILogger<YoutubeMusicService> logger, YoutubeConfig configs) {
        _logger = logger;
        _configs = configs;
        _logger.LogInformation("Initialize YoutubeMusic Service");
        _logger.LogInformation($"Base Address => {Constants.Domain}");
        _handler =new HttpClientHandler(){ 
            AutomaticDecompression = DecompressionMethods.All,
            UseDefaultCredentials = true,
            UseCookies = true,
            UseProxy = true
        };
        _client = new HttpClient(_handler);
       _client.BaseAddress = Constants.Domain;
       _client.DefaultRequestHeaders.Add("User-Agent", Constants.UserAgent);
       _client.DefaultRequestHeaders.Add("Cookie", Constants.DefaultCookie);
       Initialize();
    }
    private async void Initialize(){
        var result = await _client.GetAsync("/");
        if(result.StatusCode == System.Net.HttpStatusCode.OK){
            var content = await result.Content.ReadAsStringAsync();
            _configs.Update(content);
        }
    }

    private HttpRequestHeaders InitializeHeaders(HttpRequestHeaders header){
        string Domain = Constants.Domain.ToString();
        header.Add("Accept", "*/*");
        header.Add("User-Agent", Constants.UserAgent);
        header.Add("x-origin", Domain);
        header.Add("x-goog-vistor-id",_configs["VISITOR_DATA"]);
        header.Add("x-youtube-client-name", _configs["INNERTUBE_CLIENT_NAME", "WEB_REMIX"]);
        header.Add("x-youtube-client-version", _configs["INNERTUBE_CLIENT_VERSION","1.20220112.00.00"]);
        header.Add("x-youtube-device", _configs["DEVICE","cbr=Firefox&cbrver=72.0&ceng=Gecko&cengver=72.0&cos=Windows&cosver=10.0&cplatform=DESKTOP"]);
        header.Add("x-youtube-page-CL", _configs["PAGE_CL", "421282262"]);
        header.Add("X-YouTube-Page-Label", _configs["PAGE_BUILD_LABEL", "youtube.music.web.client_20220112_00_RC00"]);
        header.Add("Referer", Domain);
        header.Add("Cookie", _handler.CookieContainer.GetCookieHeader(Constants.Domain));
        return header;
    }
    private JObject InitializeContext(){
        JObject ret = new JObject();
        var context = _configs["INNERTUBE_CONTEXT", "{\"context\":{\"client\":{\"hl\":\"en\", \"gl\":\"KR\", \"clientName\":\"WEB_REMIX\", \"clientVersion\":\"1.20220112.00.00\"}}"];
        if(context != ""){
            ret.Add("context", JObject.Parse(context));
        } else {
            ret = JObject.FromObject(new
                {
                context = new
                {
                    client = new
                    {
                        hl = _configs["INNERTUBE_CONTEXT_HL","en"],
                        gl = _configs["INNERTUBE_CONTEXT_GL","KR"],
                        clientName = _configs["INNERTUBE_CLIENT_NAME","WEB_REMIX"],
                        clientVersion = _configs["INNERTUBE_CLIENT_VERSION","1.20220112.00.00"],
                    }
                }
            });
        }
        return ret;
    }
    private string CreateEndPoint(string endPointName) => $"youtubei/{_configs["INNERTUBE_API_VERSION", "v1"]}/{endPointName}?alt=json&key={_configs["INNERTUBE_API_KEY","AIzaSyC9XL3ZjWddXya6X74dJoCTL-WEYFDNX30"]}";

    private string CreateSearchParams(string filter, string scope, bool ignoreSpelling) {
        string param =  "";
        string param1 = "";
        string param2 = "";
        string param3 = "";
        if (filter == "" && scope == "" && !ignoreSpelling) {
            param ="EhGKAQ4IARABGAEgASgAOAFAAUICCAE%3D";
        } else {
            if (scope == "uploads") {
                param = "agIYAw%3D%3D";
            }
            if(scope == "library") {
                if(filter != ""){
                    param1 = Constants.FILTERED_PARAM1;
                    param2 = Constants.FilteredParams[filter];
                    param3 = "AWoKEAUQCRADEAoYBA%3D%3D";
                } else {
                    param = "agIYBA%3D%3D";
                }
            }
            if(scope == "" && filter != "") {
                if(filter == "playlists") {
                    param = "Eg-KAQwIABAAGAAgACgB";
                    if(!ignoreSpelling){
                        param += "MABqChAEEAMQCRAFEAo%3D";
                    } else {
                        param += "MABCAggBagoQBBADEAkQBRAK";
                    }
                } else if (filter.Contains("playlists")){
                    param1 = "EgeKAQQoA";
                    if(filter == "featured_playlists"){
                        param2 = "Dg";
                    } else { // community_playlists
                        param2 = "EA";
                    }
                    if(!ignoreSpelling) {
                        param3 = "BagwQDhAKEAMQBBAJEAU%3D";
                    } else {
                        param3 = "BQgIIAWoMEA4QChADEAQQCRAF";
                    }
                } else {
                    param1 = Constants.FILTERED_PARAM1;
                    param2 = Constants.FilteredParams[filter];
                    if(!ignoreSpelling) {
                        param3 = "AWoMEA4QChADEAQQCRAF";
                    } else {
                        param3 = "AUICCAFqDBAOEAoQAxAEEAkQBQ%3D%3D";
                    }
                }
            }
            if(param == "") {
                param = param1 + param2 + param3;
            }
        }
        return param;
    }
    private async Task<HttpResponseMessage> SendRequest(string url,JObject? Content){
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
        InitializeHeaders(request.Headers);
        _logger.LogTrace($"Request {url} Header => {request.Headers}");
        if(Content != null){
            _logger.LogTrace($"Request Content => {Content}");
            request.Content = new StringContent(Content.ToString(), Encoding.UTF8, "application/json");
        }
        var response = await _client.SendAsync(request);
        if(response.Headers.Contains("set-cookie")){
            var cookies = response.Headers.GetValues("set-cookie");
            foreach(var cookie in cookies){
                _handler.CookieContainer.SetCookies(Constants.Domain, cookie);
            }
        }
        
        return response;
    }
    public async Task<HttpResponseMessage> getSearchSuggestions(string input) {
        if(string.IsNullOrEmpty(input)) throw new ArgumentException("input parameter is null or empty");

        JObject content = InitializeContext();
        content.Add("input", input);

        return await SendRequest(CreateEndPoint("music/get_search_suggestions"), content);
    }
    public async Task<HttpResponseMessage> Search(string input, string? filter = null, string? scope = null) {
        if(string.IsNullOrEmpty(input)) throw new ArgumentException("input parameter is null or empty");

        JObject content = InitializeContext();
        content.Add("query", input);

        string parameter =  "";
        if(filter is string filterValue && scope is string scopeValue){
            if(!string.IsNullOrEmpty(filterValue) && !Constants.Filters.Contains(filterValue)) throw new ArgumentException("filter parameter not contains");
            if(!string.IsNullOrEmpty(scopeValue) && !Constants.Scopes.Contains(scopeValue)) throw new ArgumentException("scope parameter not contains");
            parameter = CreateSearchParams(filterValue, scopeValue, true);
        } else {
            parameter = CreateSearchParams("", "", false);
        }

        content.Add("params", parameter);
        return await SendRequest(CreateEndPoint("search"), content);
    }
    public void Dispose(){
        _handler.Dispose();
        _client.Dispose();
    }
}