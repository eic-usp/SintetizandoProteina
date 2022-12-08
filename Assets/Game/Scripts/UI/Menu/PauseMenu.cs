using UnityEngine;

namespace UI.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        private static bool isPaused = false;

        [SerializeField] GameObject optionMenuRef = default;
        [SerializeField] GameObject tutorialPrefab;

        public void PauseGame()
        {
            if (isPaused) return;

            this.gameObject.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }

        public void ResumeGame()
        {
            if (!isPaused) return;

            this.gameObject.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }

        public void OptionMenu()
        {
            this.gameObject.SetActive(false);
            optionMenuRef.SetActive(true);
        }

        public void OpenTutorial()
        {
            Instantiate(tutorialPrefab, transform);
        }

        public void QuitGame()
        {
            //Application.Quit();
            GameSceneManagement.Loader.Load(GameSceneManagement.Loader.Scene.UIBeg);
        }

        public bool GetIsPaused()
        {
            return isPaused;
        }
    }
}