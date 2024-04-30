using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;

/*
    Used in the marking of the game
*/

namespace UI.Text
{
    public class InfoEditableComponent : InfoComponent
    {
        [SerializeField] private List<TextMeshProUGUI> texts = default; //All the texts we need to start 
        [SerializeField] private TextMeshProUGUI labelText;

        private List<string> saveTexts = new();

        public void Setup(List<string> textsMessage, int phaseCount)
        {
            saveTexts = textsMessage;
            labelText.text = phaseCount.ToString();
        }

        public void Setup(List<TextMeshProUGUI> markingText)
        {
            texts = markingText;
        }

        //Going to be used
        public void SetText(int start){
            if (start + texts.Count > texts.Count) return;

            for (int i = 0; i < texts.Count; i++)
            {
                this.texts[i + start].text = saveTexts[i];
            }
        }
        
        public void SetText(int start, bool localized){
            
            if (start + texts.Count > texts.Count) return;
            if (!localized)
            {
                SetText(start);
                return;
            }
            
            for (int i = 0; i < texts.Count; i++)
            {
                this.texts[i + start].GetComponent<LocalizeStringEvent>().StringReference =  
                    new LocalizedString(Marking.LocalizationTable, saveTexts[i]);
            }
        }

        public void SetExternalTextVisible()
        {
            SetText(0, true);

            for (int i = 0; i < texts.Count; i++)
            {
                GameObject hold = texts[i].gameObject;
                hold.SetActive(!hold.activeSelf);
            }
        }
    }
}