namespace Networking.DataStructs
{
    [System.Serializable]
    public struct LoginData
    {
        public string emailOrId;
        public string password;
        public bool remember;
    }

    [System.Serializable]
    public struct LoginReturnData
    {
        public string token;
        public PlayerStatus player;
    }
}