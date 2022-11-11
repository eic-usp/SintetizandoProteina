using UnityEngine;
using UnityEngine.Events;
using GeneralScripts.Reflection;

namespace GeneralScripts.Player
{
    public class PlayerInfo : GeneralGetter
    {

        private string actualProtein;
        private string namePlayer;
        private float maxScore;
        private float lastScore;
        private float actualScore;

        [SerializeField] UnityEvent onAwakeEvents;


        //private string textTeste = "Al";

        //private float maxTime;
        //private float actualBestTime;
        public static PlayerInfo Instance;


        private void Awake()
        {
            // If there is an instance, do nothing 
            if (Instance == null)
            { 
                Instance = this;  
                DontDestroyOnLoad(Instance);
            }

            onAwakeEvents.Invoke();
        }

        public void SetNamePlayer(string namePlayer){Instance.namePlayer = namePlayer;}
        public void SetProteinName(string actualProtein){Instance.actualProtein = actualProtein;}
        public void SetNamePlayer(){this.namePlayer = Instance.namePlayer;}
        public void SetProteinName(){this.actualProtein = Instance.actualProtein;}

        public string GetNamePlayer() => Instance.namePlayer;
        public string GetActualProtein() => Instance.actualProtein;
    }
}