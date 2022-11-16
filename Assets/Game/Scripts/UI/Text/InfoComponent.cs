using UnityEngine;

namespace UI.Text
{
    public class InfoComponent : MonoBehaviour
    {
        [SerializeField] Transform childRef = null; //Where the information is
        private bool visibility = false;

        private void Start()
        {
            if (!childRef) return;
            childRef = transform;
        }

        public void SetAllVisible()
        {
            visibility = !visibility;

            foreach (Transform child in childRef)
            {
                child.gameObject.SetActive(visibility);
            }
        }

        public void SingleObjectVisibility()
        {
            visibility = !visibility;
            childRef.gameObject.SetActive(visibility);
        }
    }
}