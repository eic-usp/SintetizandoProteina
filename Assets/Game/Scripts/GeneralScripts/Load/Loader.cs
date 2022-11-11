using System;
using UnityEngine.SceneManagement;

namespace GameSceneManagement
{
    public static class Loader
    {
        public enum Scene
        {
            UIBeg,
            Loading,
            Gameplay
        } 

        private static Action onLoadCallback;

        public static void Load(Scene scene)
        {
            onLoadCallback = () => SceneManager.LoadScene(scene.ToString());
            SceneManager.LoadScene(Scene.Loading.ToString());
        }

        public static void LoaderCallback()
        {
            if (onLoadCallback != null)
            {
                onLoadCallback();
                onLoadCallback = null;
            }
        }
    }
}
