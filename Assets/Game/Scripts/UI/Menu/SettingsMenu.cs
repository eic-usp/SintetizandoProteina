using UnityEngine;
using UnityEngine.Audio;

namespace UI.Menu
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] AudioMixer mainMixer = default;
        [SerializeField] GameObject pauseMenuRef = default;

        private const string MasterVolume = "MasterVolume";
        private float actualVolume;

        private void Awake()
        {
            mainMixer.GetFloat(MasterVolume, out actualVolume);
        }     
        
        public void ChangeFullScreen(bool screenMode)
        {
            Screen.fullScreen = screenMode;
        }

        public void ChangeQuality(int qualityOption)
        {
            QualitySettings.SetQualityLevel(qualityOption);
        }

        public int GetQuality() => QualitySettings.GetQualityLevel();

        public void ChangeVolume(float volumeLevel)
        {
            mainMixer.SetFloat(MasterVolume, volumeLevel);
            actualVolume = volumeLevel;
        }

        public float GetVolume()
        {
            return actualVolume;
        }

        public bool GetFullScreen()
        {
            return Screen.fullScreen;
        }

        //This method will be called in the end of the drag of a slider
        //Or in a button
        public void ReleaseBasicAudioToConfirmation()
        {
            //Sound when this function is called
        }

        public void ChangeToPause()
        {
            pauseMenuRef.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}