using System.Text;

public static class YoutubeMusicHelper {
    public static string prepareLikeEndPoint(string rating){
        if(Constants.Ratings.ContainsKey(rating)){
            return Constants.Ratings[rating];
        }
        return "";
    }
    public static string prepareOrderParams(string order){
        if(Constants.Orders.ContainsKey(order)){
            return Constants.Ratings[order];
        } else {
            return "";
        }
    }
    public static async Task<string> ResponseMessageAsync(HttpResponseMessage result, Encoding encoding){
        byte[] bytes = await result.Content.ReadAsByteArrayAsync();
        var resp = encoding.GetString(bytes);
        
        return resp;
    }
    public static async Task<string> ResponseMessageAsync(HttpResponseMessage result){
        return await ResponseMessageAsync(result, Encoding.UTF8);
    }
}