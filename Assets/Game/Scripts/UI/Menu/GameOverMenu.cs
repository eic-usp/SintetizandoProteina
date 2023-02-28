using UnityEngine;

namespace UI.Menu
{
    public class GameOverMenu : MonoBehaviour
    {
        public void RestartLevel() => GameSceneManagement.Loader.Load(GameSceneManagement.Loader.Scene.Gameplay);
        public void Quit() => GameSceneManagement.Loader.Load(GameSceneManagement.Loader.Scene.UIBeg);
    }
}