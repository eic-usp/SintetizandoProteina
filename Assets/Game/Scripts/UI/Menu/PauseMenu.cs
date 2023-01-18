using UnityEngine;

namespace UI.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        public bool IsPaused { get; private set; }

        [SerializeField] private GameObject optionMenuRef;
        [SerializeField] private GameObject tutorialPrefab;

        public void PauseGame()
        {
            if (IsPaused) return;

            gameObject.SetActive(true);
            Time.timeScale = 0f;
            IsPaused = true;
        }

        public void ResumeGame()
        {
            if (!IsPaused) return;

            gameObject.SetActive(false);
            Time.timeScale = 1f;
            IsPaused = false;
        }

        public void OptionMenu()
        {
            gameObject.SetActive(false);
            optionMenuRef.SetActive(true);
        }

        public void OpenTutorial()
        {
            Instantiate(tutorialPrefab, transform);
        }

        public void QuitGame()
        {
            Time.timeScale = 1f;
            GameSceneManagement.Loader.Load(GameSceneManagement.Loader.Scene.UIBeg);
        }
    }
}