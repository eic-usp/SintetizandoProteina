using UnityEngine;
using TMPro;

namespace Phases.AMN
{
    public class AMNInputField : MonoBehaviour
    {
        [SerializeField] AMNManager amnM;
        private TMP_InputField thisInput;

        private bool wait = false;

        private void Start()
        {
            thisInput = this.GetComponent<TMP_InputField>(); 
            thisInput.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        }
        
        private async void OnSubmit()
        {
            //When there is enough characters
            //print("That it");
            print(wait + " O K " + thisInput.text);

            if (wait) return;

            if (amnM.VerifyAMN(thisInput.text))
            {
                Audio.AudioManager.Instance.Play(Audio.SoundEffectTrack.RightAnswer);
                
                string auxTextAMN = thisInput.text;
                thisInput.text = "";

                wait = true;
                thisInput.DeactivateInputField();
                await amnM.PushNewAMN(auxTextAMN);
                thisInput.ActivateInputField();
                thisInput.Select();
                wait = false;

                ValueChangeCheck();
            }
            else
            {
                Audio.AudioManager.Instance.Play(Audio.SoundEffectTrack.WrongAnswer);
            }
        }

        private void ValueChangeCheck()
        {
            if (thisInput.text.Length == 0)
            {
                return;
            }

            thisInput.text = FormatText();

            if (thisInput.text.Length == AMNManager.GetSizeAMN())
            {
                OnSubmit();
            }
        }

        private string FormatText()
        {
            if (thisInput.text.Length == 1)
            {
                return thisInput.text.ToUpper();
            }

            return thisInput.text[0].ToString().ToUpper() + thisInput.text.Substring(1).ToLower(); 
        }
    }
}