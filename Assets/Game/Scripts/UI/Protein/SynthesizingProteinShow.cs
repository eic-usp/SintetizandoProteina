using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI.Protein.Info
{
    public class SynthesizingProteinShow : MonoBehaviour
    {
        private static ProteinDescription toShow;
        
        //SynthesizingProtein
        
        [Space] [Header("Box Description of the actual synthesizing protein")] [Space]

        [SerializeField] private TextMeshProUGUI descriptionProtein;

        [Space] [Header("Box Description of Extra")] [Space]

        [SerializeField] private GameObject boxTextExtraDescription;
        [SerializeField] private TextMeshProUGUI nameExtraData;
        [SerializeField] private TextMeshProUGUI textDescriptionExtraData;
        [SerializeField] private ScrollRect scrollRectExtraData;

        private int lastExtra = -1;
        
        [Space] [Header("Destination Description")] [Space] [SerializeField]
        
        private Transform positionOfTransformedProtein;

        private void Start()
        {
            descriptionProtein.text = toShow.sp.GetDescriptionProtein();
            Instantiate(toShow.transformedProtein, positionOfTransformedProtein);
        }

        public static void SetProtein(ProteinDescription sint)
        {
            toShow = sint;
        }

        public void ShowDescriptionExtraData(int index)
        {
            ShowDescriptionExtraData("general", index);
        }
        
        public void ShowDescriptionExtraData(string key, int index)
        {
            if (key == "") return;
            if (index < 0 || index > toShow.sp.GetQtdOfExtras(key) - 1) return;
            
            if (index == lastExtra && boxTextExtraDescription.activeSelf)
            {
                lastExtra = -1;
                boxTextExtraDescription.SetActive(false);
                return;
            }
            
            lastExtra = index;
            boxTextExtraDescription.SetActive(true);
            
            nameExtraData.text = toShow.sp.GetNameExtra(key, index);
            textDescriptionExtraData.text = toShow.sp.GetDescriptionTextExtra(key, index);
            scrollRectExtraData.verticalNormalizedPosition = 1f;
        }

        public TextMeshProUGUI GetDescriptionProtein() => descriptionProtein;
    }
}