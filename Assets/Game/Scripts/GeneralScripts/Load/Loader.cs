using System;
using UnityEngine.SceneManagement;

namespace GameSceneManagement
{
    public static class Loader
    {
        public enum Scene
        {
            None = -1,
            UIBeg,
            Loading,
            Gameplay
        } 

        private static Action onLoadCallback;

        public static void Load(Scene scene)
        {
            if (scene == Scene.None) return;
            onLoadCallback = () => SceneManager.LoadScene(scene.ToString());
            Audio.AudioManager.Instance.Play(scene);
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
