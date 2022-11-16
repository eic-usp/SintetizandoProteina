using UnityEngine;
using UnityEngine.EventSystems;
using Phases;

/*
    It seems a little strange or useless, but it's a lot useful
    It pretty much sends to the Controller (Onwer) the Index of this child
*/

public class InputFieldChild : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] bool validInputField = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!validInputField) return;
        this.transform.parent.GetComponent<TextWithInput>().SendInput();
    }
}