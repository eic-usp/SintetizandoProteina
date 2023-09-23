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
            url = url.Trim();
            
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("Invalid URL");
                return;
            }
        
            Application.OpenURL(url);
        }
    }
}
