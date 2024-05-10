using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.Localization.Components;

namespace Phases.Processing
{
    public class ProcessingManager : PhaseManagerMono
    {
        [Space] [Header("Processing Manager Variables")] [Space]
        [SerializeField] private Cell.CellAnimator cellReference; //Used for the single purpose of animation
        [SerializeField] private GameObject processingDescriptionObject; 
        [SerializeField] private LocalizeStringEvent processingDescriptionTextLocalized;
        [SerializeField] private Button continueButton;

        [SerializeField] private string localizationTableKey = "Quiz";
        private void Start()
        {
            Setup();
            processingDescriptionObject.SetActive(true);
            cellReference.SetAnimatorStatus(true);
            PlayAMNQueueTransformation();
            continueButton.onClick.AddListener(EndPhase);
        }

        private void Setup()
        {
            var protein = FindObjectOfType<GeneralScripts.Player.PlayerInfo>().ProteinName;
            //processingDescriptionText.text = Resources.Load<TextAsset>("ProcessingDescription/" + protein).text;

            LocalizationManager.SetStringReference(processingDescriptionTextLocalized, localizationTableKey,
                Resources.Load<TextAsset>("ProcessingDescription/" + protein).text);
        }

        private async void PlayAMNQueueTransformation()
        {
            var time = cellReference.AMNTransformation();
            await UniTask.Delay(Util.ConvertToMili(time / 0.5f));
        }

        public new void EndPhase()
        {
            base.EndPhase();
        }
    }
}