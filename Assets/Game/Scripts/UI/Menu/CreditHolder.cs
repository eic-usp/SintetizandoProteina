using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace UI.Menu
{
    public class CreditHolder : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent contentString;

        public void Setup(string groupName)
        {
            LocalizationManager.SetStringReference(contentString, LocalizationManager.DefaultTable,groupName);
        }
    }
}