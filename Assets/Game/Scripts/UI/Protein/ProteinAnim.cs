using UnityEngine;
using UnityEngine.EventSystems;

/*
    Control the Protein so it don't rotate at the start of the game
*/

namespace UI.Protein.Info
{
    public class ProteinAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        Animator anim;

        int rotationHash = Animator.StringToHash("Rotation");

        void Start(){
            anim = GetComponent<Animator>();
            anim.enabled = false;
        }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            anim.enabled = true;
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            anim.enabled = false;
        }
    }
}