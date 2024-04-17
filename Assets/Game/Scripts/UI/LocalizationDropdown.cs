using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class LocalizationDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown tmpDropdown;
        [SerializeField] private Keys[] keys;
        
        [Serializable]
        public struct Keys
        {
#if UNITY_EDITOR
            public string name;
#endif
            
            public string table;
            public string key;
        }

        private void Start()
        {
            AddOptions();
        }
        
        public void LocalizeOptionsText()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                tmpDropdown.options[i].text = LocalizationManager.FindStringReference(keys[i].table, keys[i].key).Value;
            }

            tmpDropdown.captionText.text = tmpDropdown.options[GetDropdownValue()].text;
        }
        
        private void AddOptions()
        {
            tmpDropdown.ClearOptions();
            var dropdownOptions = new List<TMP_Dropdown.OptionData>();

            foreach (var key in keys)
            {
                var text = LocalizationManager.FindStringReference(key.table, key.key).Value;
                
                dropdownOptions.Add(new TMP_Dropdown.OptionData(text));
            }
            
            tmpDropdown.AddOptions(dropdownOptions);
            tmpDropdown.captionText.text = tmpDropdown.options[GetDropdownValue()].text;

        }
        
        public int GetDropdownValue()
        {
            return tmpDropdown.value;
        }

    }
}
