public interface IYoutubeMusicService : IDisposable {
        Task<HttpResponseMessage> getSearchSuggestions(string input);
        Task<HttpResponseMessage> Search(string input, string? filter, string? scope);
}