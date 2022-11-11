using UnityEngine;
using TMPro;

namespace UI.Menu
{
    public class QualityOption : MonoBehaviour
    {
        [SerializeField] SettingsMenu sm = default;

        private TMP_Dropdown dropdown;

        private void Start()
        {
            dropdown = this.transform.GetComponentInChildren<TMP_Dropdown>(true);
            dropdown.value = sm.GetQuality();
        }
    }
}