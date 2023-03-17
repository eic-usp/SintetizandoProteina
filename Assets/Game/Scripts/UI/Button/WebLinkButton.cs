using UnityEngine;

namespace UI.Button
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class WebLinkButton : MonoBehaviour
    {
        [SerializeField] private string url;
    
        private void Start()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Open);
        }

        private void Open()
        {
            if (string.IsNullOrEmpty(url.Trim()))
            {
                Debug.LogError("Invalid URL");
            }
        
            Application.OpenURL(url);
        }
    }
}
