using UnityEngine;
using TMPro;

namespace UI.Menu
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
    
        private void OnEnable()
        {
            scoreText.text = 
                $"{LocalizationManager.FindStringReference(LocalizationManager.MainMenuAndGameplayRandoms, "id_ending_congratulation").Value}{ScoreManager.Instance.Score}{LocalizationManager.FindStringReference(LocalizationManager.MainMenuAndGameplayRandoms, "id_ending_congratulation_points").Value}";
        }
        
        public void BackToMenu() => GameSceneManagement.Loader.Load(GameSceneManagement.Loader.Scene.UIBeg);
        public void Quit() => Application.Quit();
    }
}