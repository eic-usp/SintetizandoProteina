public static class Endpoints
{
    public static string Base { private get; set; }
    public static string GameId { private get; set; }
    public static string Login => $"{Base}/player/login";
    public static string Validate => $"{Base}/player/validate";
    public static string Match => $"{Base}/game/{GameId}/match";
    public static string PlayerStatus => $"{Base}/player";
    public static string PlayerRanking => $"{Base}/player/{GameId}/ranking";
    public static string Rankings => $"{Base}/game/{GameId}/rankings";
}