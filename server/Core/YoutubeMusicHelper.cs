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

}