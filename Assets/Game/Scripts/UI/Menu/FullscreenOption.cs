using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Settings
{
    using Button = UnityEngine.UI.Button;

    public class FullscreenOption : MonoBehaviour
    {

        [SerializeField] SettingsMenu sm = default;
        [SerializeField] Button onOption = default;
        [SerializeField] Button offOption = default;

        private void Start()
        {
            if (sm.GetFullScreen())
            {
                offOption.onClick.Invoke();
                return;
            }

            onOption.onClick.Invoke();
        }
    }
}