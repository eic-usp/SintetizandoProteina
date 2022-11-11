using UnityEngine;
using TMPro;

/*Pretty much just a text component*/

namespace UI.Text
{
    public class Letter : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI letterText = null;
        public void Setup(string value)
        {
            if (letterText == null)
            {
                transform.GetComponentInChildren<TextMeshProUGUI>().text = value;
                return;
            }

            letterText.text = value;
        }

        public string GetValue()
        {
            if (letterText == null)
            {
                return transform.GetComponentInChildren<TextMeshProUGUI>().text;
            }

            return letterText.text;
        }
    }
}