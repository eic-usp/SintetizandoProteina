using UnityEngine;
using TMPro;

namespace UI.Menu
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
    
        private void OnEnable()
        {
            scoreText.text = $"Parabéns! Você fez <color=#FCA065><u>{ScoreManager.Instance.Score}</u></color> pontos!";
        }
        
        public void BackToMenu() => GameSceneManagement.Loader.Load(GameSceneManagement.Loader.Scene.UIBeg);
        public void Quit() => Application.Quit();
    }
}