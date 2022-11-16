using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
    Used in the marking of the game
*/

namespace UI.Text
{
    public class InfoEditableComponent : InfoComponent
    {
        [SerializeField] List<TextMeshProUGUI> texts = default; //All the texts we need to start 

        private List<string> saveTexts = new List<string>();

        public void Setup(List<string> textsMessage)
        {
            saveTexts = textsMessage;
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

        public void SetExternalTextVisible()
        {
            SetText(0);

            for (int i = 0; i < texts.Count; i++)
            {
                GameObject hold = texts[i].gameObject;
                hold.SetActive(!hold.activeSelf);
            }
        }
    }
}