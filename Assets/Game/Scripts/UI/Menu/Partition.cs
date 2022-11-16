using UnityEngine;

//Kinda unnecessary, because the buttons in Unity can do this, just a reminder 

namespace UI.Menu
{
    public class Partition : MonoBehaviour
    {
        protected int index;
        [SerializeField] protected SelectionManager selection;

        private void Start()
        {
            index = transform.GetSiblingIndex();

            if (!selection)
            {
                selection = this.transform.parent.GetComponent<SelectionManager>();
            }
        }
        
        public void Resume()
        {
            selection.ChangePartitionByIndex(index);
        }
        
    }
}