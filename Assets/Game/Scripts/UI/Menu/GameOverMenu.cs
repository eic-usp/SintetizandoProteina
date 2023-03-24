using UnityEngine;

namespace UI.Menu
{
    public class GameOverMenu : MonoBehaviour
    {
        public void BackToMenu() => GameSceneManagement.Loader.Load(GameSceneManagement.Loader.Scene.UIBeg);
        public void Quit() => Application.Quit();
    }
}