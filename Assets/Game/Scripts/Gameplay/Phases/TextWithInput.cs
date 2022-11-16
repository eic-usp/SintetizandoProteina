using UnityEngine;
using TMPro;
using UI.Text;

/*
    Needs a input field child to work with
    See SendInput function in this class
*/

namespace Phases
{
    public class TextWithInput : Letter
    {
        protected int originalPosition; //Works fine, can be useful later
        [SerializeField] TMP_InputField mainInputField; //Used in the prefabs

        public static InputPhase owner;

        public static void SetOnwer(InputPhase on)
        {
            TextWithInput.owner = on; //It's static, but it could be private only 
        }

        public void SendInput()
        {
            owner.SetActualInput(transform.GetSiblingIndex());
        }

        public void SetPosition(int index)
        {
            originalPosition = index;
        }

        public int GetOriginalPosition() => originalPosition;
        
        public void ActivateInput()
        {
            mainInputField.ActivateInputField();
        }

        public void DeactivateInput()
        {
            mainInputField.DeactivateInputField();
        }

        public TMP_InputField GetMainInputField() => mainInputField;

        public void SetValueInputText(string value)
        {
            this.mainInputField.text = value;
        }

        public string GetValueInputText() => mainInputField.text;
    }
}