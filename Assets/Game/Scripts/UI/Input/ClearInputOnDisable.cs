using TMPro;
using UnityEngine;

namespace UI.Input
{
    [RequireComponent(typeof(TMP_InputField))]
    public class ClearInputOnDisable : MonoBehaviour
    {
        private TMP_InputField _inputField;
        
        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
        }
        
        private void OnDisable()
        {
            _inputField.text = string.Empty;
        }
    }
}