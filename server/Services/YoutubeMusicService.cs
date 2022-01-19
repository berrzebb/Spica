using Newtonsoft.Json.Linq;

public class YoutubeMusicService : IYoutubeMusicService {
    readonly YoutubeMusicClientConfig _config;
    HttpClient _client;
    public YoutubeMusicService(YoutubeMusicClientConfig config) {
        Console.WriteLine("Initialize YoutubeMusic Service");
        Console.WriteLine($"Base Address => {config.BaseAddress}");
        _config = config;
        _client = new HttpClient();
       _client.BaseAddress = new Uri(_config.Domain);
        _client.DefaultRequestHeaders.Add("User-Agent", Constants.UserAgent);
        _client.DefaultRequestHeaders.Add("Accept", "*/*");
        //_client.DefaultRequestHeaders.Add("Content-Type", "application/json");
        _client.DefaultRequestHeaders.Add("X-Goog-AuthUser", "0");
        _client.DefaultRequestHeaders.Add("x-origin", _config.Domain);
        _client.DefaultRequestHeaders.Add("Cookie", _config.Cookie);
    }
    private JObject InitializeContext(){
        return JObject.FromObject(new {
            context = new {
                client = new {
                    browserName = "Chrome",
                    browserVersion = "97.0.4692.71",
                    clientName = _config.ClientName,
                    clientVersion = _config.ClientVersion,
                    deviceMake = "",
                    deviceModel =  "",
                    experimentIds = new List<string>(),
                    experimentsToken = "",
                    gl = _config.gl,
                    hl = _config.hl,
                    locationInfo = new {
                        locationPermissionAuthorizationStatus = "LOCATION_PERMISSION_AUTHORIZATION_STATUS_UNSUPPORTED",
                    },
                    musicAppInfo = new {
                        musicActivityMasterSwitch = "MUSIC_ACTIVITY_MASTER_SWITCH_INDETERMINATE",
                        musicLocationMasterSwitch = "MUSIC_LOCATION_MASTER_SWITCH_INDETERMINATE",
                        pwaInstallabilityStatus = "PWA_INSTALLABILITY_STATUS_UNKNOWN",
                    },
//                    utcOffsetMinutes = -TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now)
                },
                request = new {
                    consistencyTokenJars = new List<string>(),
                    internalExperimentFlags = new List<string>(),
                    useSsl = true
            },
                user = new {
                    lockedSafetyMode = false
                }
            }
        });
    }
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
    private JObject CreateSuggestStats(string query) {
        return JObject.FromObject(new {
            clientName = _config.ClientName,
            inputMethod = "KEYBOARD",
            originalQuery = query,
            parameterValidationStatus = "VALID_PARAMETERS",
            searchMethod = "ENTER_KEY",
            validationStatus = "VALID",
            zeroPrefixEnabled = true
        });
    }

    private async Task<HttpResponseMessage> createAPIRequest(string endPointName, JObject Content){
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseDomain}{endPointName}?{_config.QueryString}");
        Console.WriteLine($"Request Content => {Content.ToString()}");
        ;
        request.Content = new StringContent(Content.ToString());
        return await _client.SendAsync(request);
    }
    public async Task<HttpResponseMessage> getSearchSuggestions(string input) {
        JObject content = InitializeContext();
        content.Add("input", input);
        return await createAPIRequest("get_search_suggestions", content);
    }
    public async Task<HttpResponseMessage> Search(string input, string filter = "songs", string scope = "library") {
        if((filter != "" || scope != "") && (!Constants.Filters.Contains(filter) || !Constants.Scopes.Contains(scope))) return new HttpResponseMessage();
        JObject content = InitializeContext();
        content.Add("query", input);
        content.Add("params", CreateSearchParams(filter, scope, true));
        content.Add("suggestStats", CreateSuggestStats(input));
        return await createAPIRequest("search", content);
    }
    public void UpdateCookie(string cookie) {
        _client.DefaultRequestHeaders.Add("Cookie", cookie);
    }
    public void Dispose(){
        _client.Dispose();
    }
}