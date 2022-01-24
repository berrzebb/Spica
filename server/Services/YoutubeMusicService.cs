using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

public class YoutubeMusicService : IYoutubeMusicService {
    HttpClientHandler _handler;
    HttpClient _client;
   JObject _ytcfg = new JObject();
    public YoutubeMusicService() {
        Console.WriteLine("Initialize YoutubeMusic Service");
        Console.WriteLine($"Base Address => {Constants.Domain}");
        _handler =new HttpClientHandler(){ 
            AutomaticDecompression = DecompressionMethods.All,
            UseDefaultCredentials = true,
            UseCookies = true,
            UseProxy = true
        };
        _client = new HttpClient(_handler);
       _client.BaseAddress = new Uri(Constants.Domain);
       _client.DefaultRequestHeaders.Add("User-Agent", Constants.UserAgent);
       _client.DefaultRequestHeaders.Add("Cookie", Constants.DefaultCookie);
       //_client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.5");
       //InitializeHeaders(_client.DefaultRequestHeaders);
       Initialize();
    }
    private async void Initialize(){
        var result = await _client.GetAsync("/");
        if(result.StatusCode == System.Net.HttpStatusCode.OK){
            var content = await result.Content.ReadAsStringAsync();
            var matches = Regex.Matches(content, "ytcfg\\.set\\s*\\(\\s*({.+?})\\s*\\)\\s*;");
            var match = matches[0].ToString();
            match = match.Replace("ytcfg.set(","").Replace(");", "");
            _ytcfg = JObject.Parse(match);
        }
    }
    private string GetYoutubeMusicConfig(string key, string defaultvalue = ""){
        if(_ytcfg.TryGetValue(key,out JToken? value)){
            return value.ToString();
        }else {
            Console.WriteLine($"Youtube Music Config Missing Parameter => {key}");
            return "";
        }
    }
    private TimeZoneInfo timeZone => TimeZoneInfo.Local;
    private double GetUtcOffset() => timeZone.GetUtcOffset(DateTimeOffset.UtcNow).TotalSeconds;
    private string GetTimeZone() => timeZone.DisplayName;
    private HttpRequestHeaders InitializeHeaders(HttpRequestHeaders header){
        header.Add("Accept", "*/*");
        //header.Add("Accept-Encoding", "gzip,deflate,br");
        header.Add("User-Agent", Constants.UserAgent);
        header.Add("x-origin", Constants.Domain);
//        header.Add("x-goog-pageid",GetYoutubeMusicConfig("PAGE_ID"));
        header.Add("x-goog-vistor-id",GetYoutubeMusicConfig("VISITOR_DATA"));
        header.Add("x-youtube-client-name", GetYoutubeMusicConfig("INNERTUBE_CLIENT_NAME", "WEB_REMIX"));
        header.Add("x-youtube-client-version", GetYoutubeMusicConfig("INNERTUBE_CLIENT_VERSION","1.20220112.00.00"));
        header.Add("x-youtube-device", GetYoutubeMusicConfig("DEVICE","cbr=Firefox&cbrver=72.0&ceng=Gecko&cengver=72.0&cos=Windows&cosver=10.0&cplatform=DESKTOP"));
        header.Add("x-youtube-page-CL", GetYoutubeMusicConfig("PAGE_CL", "421282262"));
        header.Add("X-YouTube-Page-Label", GetYoutubeMusicConfig("PAGE_BUILD_LABEL", "youtube.music.web.client_20220112_00_RC00"));
        //header.Add("X-YouTube-Utc-Offset", (-GetUtcOffset()).ToString());
        //header.Add("X-YouTube-Time-Zone", GetTimeZone());
        header.Add("Referer", Constants.Domain);
        header.Add("Cookie", _handler.CookieContainer.GetCookieHeader(new Uri(Constants.Domain)));
        return header;
    }
    private JObject InitializeContext(){
        JObject ret = new JObject();
        var context = GetYoutubeMusicConfig("INNERTUBE_CONTEXT", "{\"context\":{\"client\":{\"hl\":\"en\", \"gl\":\"KR\", \"clientName\":\"WEB_REMIX\", \"clientVersion\":\"1.20220112.00.00\"}}");
        if(context != ""){
            ret.Add("context", JObject.Parse(context));
        } else {
            ret = JObject.FromObject(new
                {
                context = new
                {
                    client = new
                    {
                        hl = GetYoutubeMusicConfig("INNERTUBE_CONTEXT_HL","en"),
                        gl = GetYoutubeMusicConfig("INNERTUBE_CONTEXT_GL","KR"),
                        clientName = GetYoutubeMusicConfig("INNERTUBE_CLIENT_NAME","WEB_REMIX"),
                        clientVersion = GetYoutubeMusicConfig("INNERTUBE_CLIENT_VERSION","1.20220112.00.00"),
                    }
                }
            });
        }
        return ret;
    }
    private string CreateEndPoint(string endPointName) => $"youtubei/{GetYoutubeMusicConfig("INNERTUBE_API_VERSION", "v1")}/{endPointName}?alt=json&key={GetYoutubeMusicConfig("INNERTUBE_API_KEY","AIzaSyC9XL3ZjWddXya6X74dJoCTL-WEYFDNX30")}";

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
        Console.WriteLine($"Request {url} Header => {request.Headers}");
        if(Content != null){
            Console.WriteLine($"Request Content => {Content}");
            request.Content = new StringContent(Content.ToString(), Encoding.UTF8, "application/json");
        }
        var response = await _client.SendAsync(request);
        if(response.Headers.Contains("set-cookie")){
            var cookies = response.Headers.GetValues("set-cookie");
            foreach(var cookie in cookies){
                Console.WriteLine($"{url} => cookie => {cookie}");
            }
        }
        return response;
    }
    public async Task<HttpResponseMessage> getSearchSuggestions(string input) {
        if(input == "") return new HttpResponseMessage();
        JObject content = InitializeContext();
        content.Add("input", input);
        return await SendRequest(CreateEndPoint("music/get_search_suggestions"), content);
    }
    public async Task<HttpResponseMessage> Search(string input, string? filter = "", string? scope = "") {
        if((filter != "" || scope != "") && (!Constants.Filters.Contains(filter ?? "") || !Constants.Scopes.Contains(scope ?? ""))) return new HttpResponseMessage();
        JObject content = InitializeContext();
        content.Add("query", input);
        if(filter != "" && scope != ""){
            content.Add("params", CreateSearchParams(filter ?? "", scope  ?? "", true));
        }
        return await SendRequest(CreateEndPoint("search"), content);
    }
    public void Dispose(){
        _handler.Dispose();
        _client.Dispose();
    }
}