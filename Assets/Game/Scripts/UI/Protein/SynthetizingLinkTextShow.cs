using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Protein.Info
{
    using Input = UnityEngine.Input;
    public class SynthetizingLinkTextShow : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] SynthesizingProteinShow synthesizingProteinShow;

        public void OnPointerClick(PointerEventData eventData)
        {
            int index = TMP_TextUtilities.FindIntersectingLink(synthesizingProteinShow.GetDescriptionProtein(), Input.mousePosition, Camera.main);

            if (index > -1)
            {
                synthesizingProteinShow.ShowDescriptionExtraData(index);
            }
        }
    }
}
