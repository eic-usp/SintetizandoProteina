using UnityEngine;
using UnityEngine.Events;
using GeneralScripts.Reflection;

namespace GeneralScripts.Player
{
    public class PlayerInfo : GeneralGetter
    {
        private string proteinName;
        private string proteinDisplayName;
        private string namePlayer;
        private float maxScore;
        private float lastScore;
        private float actualScore;

        [SerializeField] UnityEvent onAwakeEvents;
        
        public static PlayerInfo Instance;
        
        private void Awake()
        {
            // If there is an instance, do nothing 
            if (Instance == null)
            { 
                Instance = this;  
                DontDestroyOnLoad(Instance);
            }

            onAwakeEvents?.Invoke();
        }

        public void SetNamePlayer(string namePlayer)
        {
            Instance.namePlayer = namePlayer;
        }

        public void SetProteinName(string proteinName)
        {
            Instance.proteinName = proteinName;
        }
        
        public void SetProteinDisplayName(string proteinDisplayName)
        {
            Instance.proteinDisplayName = proteinDisplayName;
        }

        public void SetNamePlayer()
        {
            namePlayer = Instance.namePlayer;
        }
        
        public void SetProteinName()
        {
            proteinName = Instance.proteinName;
        }

        public void SetProteinDisplayName()
        {
            proteinDisplayName = Instance.proteinDisplayName;
        }

        public string NamePlayer => Instance.namePlayer;
        public string ProteinDisplayName => Instance.proteinDisplayName;
        public string ProteinName => Instance.proteinName;
    }
}