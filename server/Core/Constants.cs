public static class Constants {
    public static readonly string Domain = "https://music.youtube.com"; 
    public static readonly HashSet<string> Filters = new HashSet<string>(){
        "albums",
        "artists",
        "playlists",
        "community_playlists",
        "featured_playlists",
        "songs",
        "videos"
    };
    public static readonly HashSet<string> Scopes = new HashSet<string>(){
        "library",
        "uploads",
    };
    public static readonly Dictionary<string,string> Orders = new Dictionary<string, string>(){
        {"a_to_z", "ggMGKgQIARAA"},
        {"z_to_a", "ggMGKgQIARAB"},
        {"recently_added", "ggMGKgQIABAB"}
    };
    public static readonly Dictionary<string,string> Ratings = new Dictionary<string, string>(){
        {"LIKE", "like/like"},
        {"DISLIKE", "like/dislike"},
        {"INDIFFERENT", "like/removelike"}
    };

    public static readonly Dictionary<string, string> FilteredParams = new Dictionary<string, string>(){
        {"songs", "I"},
        {"videos", "Q"},
        {"albums", "Y"},
        {"artists", "g"},
        {"playlists", "o"},
    };
    public static readonly string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:72.0) Gecko/20100101 Firefox/72.0";
    public static readonly string FILTERED_PARAM1 = "EgWKAQI";
    public static readonly string DefaultCookie ="YSC=BfubbR6H7DA; VISITOR_INFO1_LIVE=Wn53tgoHrK0; HSID=Aejp3c984FVxVZr2e; SSID=A7ZSPT5rt8x9DPw1g; APISID=3q_UE4bk9X2KMk0E/A1aLrBWUPWC75DTU_; SAPISID=p6gXxX8Veg05UEGZ/AQ2u55vTp9C4rwj1P; __Secure-1PAPISID=p6gXxX8Veg05UEGZ/AQ2u55vTp9C4rwj1P; __Secure-3PAPISID=p6gXxX8Veg05UEGZ/AQ2u55vTp9C4rwj1P; LOGIN_INFO=AFmmF2swRQIgQ-yd6_lXE8sDiuty8a9_XFZQTjeiqgqJumuvYW2p5pICIQCLpHx6ednIlqbXkZ9MdjELGyKaQQceWcZw3AnckcztJw:QUQ3MjNmeDY0MmlzUUI5WF91SHVqX2FlaXZwdnpIcXUtbjRENHFoVlpEd3JtVGs1bU1XYkhFeEh6UnpEOUt4OTNVdmgyQTVLTU5kSml1RmJ4c0J3LVNmNElxU21WYkFpVkpKVzRvYUFCMUZ6TGRkS185WjEtbWthTmRHM3Y0WjNHVmVzS29fQXlRdDU4Zk10eEZYMm43ODg4U1k4dU5pWUpHdmZjZ3dhaFJsR3pyT2lXUWRrZFVmbEREdW1YWjM1TzFBR2xuOFhWMHdzaS1vMEVaNHBuN0FUVGZrekI5d2gwZw==; SID=FQiB7m0v_uTMODCcQ3QPrkju744lN7HyLZy77N41GgIGSJ1VNz_LgsQzuTIV8kWSdEUfIQ.; __Secure-1PSID=FQiB7m0v_uTMODCcQ3QPrkju744lN7HyLZy77N41GgIGSJ1V4y8TNmAm4VYdsEV8KBwV9g.; __Secure-3PSID=FQiB7m0v_uTMODCcQ3QPrkju744lN7HyLZy77N41GgIGSJ1V51odOisi5V7fTWaWKs1gaw.; _gcl_au=1.1.1255233684.1642565770; PREF=f6=40000080&tz=Asia.Seoul&volume=100&repeat=ALL; SIDCC=AJi4QfGSyDk2Ug2V39SMDV3NfV6Bc8rUJlNRnSSt70Eyf3MqXQN0qEHw5wOd6dTpOdowCckigKo; __Secure-3PSIDCC=AJi4QfGoxSU0LKXEYd8CCWc-YUR_b-uesRiWmL7rEwjEeZmfNd8nh8HznjCL5-BlWxMkGgPTsxo";

}
