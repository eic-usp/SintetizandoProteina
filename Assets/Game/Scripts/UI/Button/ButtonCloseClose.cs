using UnityEngine;
using UnityEngine.Events;

namespace UI.Button
{
    /*
        The main idea of this script is to open X things and close Y things

        But in can be used in N diferrent ways
    */

    public class ButtonCloseClose : MonoBehaviour
    {
        //Idea from Thiago Bandeira Testing

        [SerializeField] UnityEvent click;
        [SerializeField] UnityEvent afterClick;

        bool clicked = true; //See the beginning of the OnClickButton, it turn to be false actually

        public void OnClickButton()
        {
            ChangeState();
            
            if (clicked)
            {
                afterClick.Invoke();
                return;
            }

            click.Invoke();
        }

        public void ChangeState()
        {
            clicked = !clicked;
        }
    }
}