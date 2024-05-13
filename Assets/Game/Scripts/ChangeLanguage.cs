using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Game.Scripts
{
    public class ChangeLanguage : MonoBehaviour
    {
        [SerializeField] private int id;

        private bool _active = false;
        
        public void ChangeLocale()
        {
            StartCoroutine(SetLocale());
        }

        public IEnumerator SetLocale()
        {
            _active = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[id];
            _active = false;
        }
    }
}
