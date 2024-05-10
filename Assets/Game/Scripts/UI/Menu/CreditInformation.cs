using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace UI.Menu
{
    public class CreditInformation : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameCredit;
        [SerializeField] private LocalizeStringEvent contentString;
        [SerializeField] private Image imageCredit;

        public void Setup(string nameCredit, string contentCredit, Sprite imageCredit)
        {
            this.nameCredit.text = nameCredit;
            LocalizationManager.SetStringReference(contentString, LocalizationManager.DefaultTable,contentCredit);

            this.imageCredit.sprite = imageCredit;
        }
    }
}