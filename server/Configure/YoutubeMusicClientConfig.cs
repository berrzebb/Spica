public class YoutubeMusicClientConfig {
    public string Domain {get; set;} = "https://music.youtube.com";
    public string BaseDomain {get;set;}  = $"/youtubei/v1";
    public string BaseAddress => $"{Domain}{BaseDomain}";
    public string Cookie {get; set;} = "";
    public string ClientName {get; set;} = "WEB_REMIX";
    public string ClientVersion {get; set; } = "1.20220112.00.00";
    public List<String> experimentIds {get; set;} = new List<String>();
    public string experimentsToken {get; set;} = "";
    public string gl {get; set;} = "KR";
    public string hl {get; set;} = "en";
    
    public string QueryString {get; set;} = "alt=json&key=AIzaSyC9XL3ZjWddXya6X74dJoCTL-WEYFDNX30";

}