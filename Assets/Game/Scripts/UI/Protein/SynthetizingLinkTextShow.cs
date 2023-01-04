using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Protein.Info
{
    using Input = UnityEngine.Input;
    public class SynthetizingLinkTextShow : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] SynthesizingProteinShow synthesizingProteinShow;
        [SerializeField] private SynthetizingLinkTextShow extraBox;
        [SerializeField] private string subject;

        public void SetSubject(string subject) => this.subject = subject;

        public void OnPointerClick(PointerEventData eventData)
        {
            extraBox.SetSubject(subject);
            Debug.Log("Pointer click");
            int index = TMP_TextUtilities.FindIntersectingLink(GetComponent<TextMeshProUGUI>(), Input.mousePosition, Camera.main);

            if (index > -1)
            {
                synthesizingProteinShow.ShowDescriptionExtraData(subject, index);
            }
            
            Debug.Log($"index: {index}");
        }
    }
}
