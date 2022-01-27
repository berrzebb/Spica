using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

public class YoutubeConfig {
   private JObject configure = new JObject();
   private readonly ILogger _logger;
    public YoutubeConfig(ILogger<YoutubeConfig> logger) {
        _logger = logger;
    }
    public void Update(string config){
            var matches = Regex.Matches(config, "ytcfg\\.set\\s*\\(\\s*({.+?})\\s*\\)\\s*;");
            if(matches.Count == 0) return;
            var match = matches[0].ToString();
            match = match.Replace("ytcfg.set(","").Replace(");", "");
            configure = JObject.Parse(match);
    }
    private string GetYoutubeMusicConfig(string key, string defaultvalue = ""){
        if(configure.TryGetValue(key, out JToken? value)){
            return value.ToString();
        }else {
            _logger.LogInformation($"Youtube Music Config Missing Parameter => {key}");
            return defaultvalue;
        }
    }

    public string this[string key, string defaultValue = ""]{
        get => GetYoutubeMusicConfig(key, defaultValue);
    }
}
