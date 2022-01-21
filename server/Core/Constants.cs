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

}
