using UnityEngine;
using UnityEngine.Events;

namespace UI.Input
{
    using Input = UnityEngine.Input;
    
    public class InputSection : MonoBehaviour
    {
        [SerializeField] UnityEvent submit;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                submit.Invoke();
            }
        }
    }
}