namespace Networking.DataStructs
{
    [System.Serializable]
    public struct LoginData
    {
        public string emailOrId;
        public string password;
    }

    [System.Serializable]
    public struct LoginReturnData
    {
        public string token;
        public PlayerStatus player;
    }
}