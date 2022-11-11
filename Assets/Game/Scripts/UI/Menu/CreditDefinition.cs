using System.Collections.Generic;
using UnityEngine;

namespace UI.Menu
{
    public class CreditDefinition : MonoBehaviour
    {
        [SerializeField] private CreditHolder creditInformationHolder;
        [SerializeField] private CreditInformation creditPrefab;
        [SerializeField] private Transform spawnGroups;
        [SerializeField] private List<CreditGroups> groups;

        [System.Serializable]
        private class CreditGroups
        {
            public string groupName;
            public List<CreditGroupInformation> creditGroupInformation;
        }

        [System.Serializable]
        private class CreditGroupInformation
        {
            public string nameCredit;
            public string contentCredit;
            public Sprite imageCredit;
        }

        private void Awake()
        {
            foreach (var credit in groups)
            {
                var holder = Instantiate(creditInformationHolder, spawnGroups);
                holder.Setup(credit.groupName);
                
                foreach (var information in credit.creditGroupInformation)
                {
                    var holderInfo = Instantiate(creditPrefab, holder.transform);
                    holderInfo.Setup(information.nameCredit, information.contentCredit, information.imageCredit);
                }
            }
        }
    }
}