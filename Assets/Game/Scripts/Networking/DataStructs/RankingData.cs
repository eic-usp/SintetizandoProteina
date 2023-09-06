namespace Networking.DataStructs
{
    [System.Serializable]
    public struct RankingEntry
    {
        public string playerId;
        public int highScore;
        public string playedAt;
    }

    [System.Serializable]
    public struct RankingData
    {
        public RankingEntry[] entries;
    }
}