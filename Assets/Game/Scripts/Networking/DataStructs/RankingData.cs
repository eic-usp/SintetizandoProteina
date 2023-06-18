namespace Networking.DataStructs
{
    [System.Serializable]
    public struct RankingEntry
    {
        public string name;
        public int score;
    }

    [System.Serializable]
    public struct RankingData
    {
        public RankingEntry[] entries;
    }
}