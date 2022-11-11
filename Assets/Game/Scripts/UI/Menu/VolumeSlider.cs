using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] SettingsMenu sm = default;

        private Slider slider;

        private void Start()
        {
            slider = this.transform.GetComponent<Slider>();
            slider.value = sm.GetVolume();
        }
    }
}