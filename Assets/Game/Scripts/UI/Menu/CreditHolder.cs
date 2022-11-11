using TMPro;
using UnityEngine;

namespace UI.Menu
{
    public class CreditHolder : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI groupName;

        public void Setup(string groupName)
        {
            this.groupName.text = groupName;
        }
    }
}