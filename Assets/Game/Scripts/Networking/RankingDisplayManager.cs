using Networking.DataStructs;
using Networking.RequestHandlers;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
    public class RankingDisplayManager : MonoBehaviour
    {
        [SerializeField] private Transform rankingEntriesPanel;
        [SerializeField] private ScrollRect rankingScrollView;
        [SerializeField] private Button anchorButton;

        [Header("Ranking Display Entry Objects")]
        
        [SerializeField] private RankingDisplayEntry rankingDisplayEntry;
        [SerializeField] private RankingDisplayEntry rankingDisplayEntryFirstPlace;
        [SerializeField] private RankingDisplayEntry rankingDisplayEntrySecondPlace;
        
        [SerializeField] private RankingDisplayEntry rankingDisplayEntrySelf;
        [SerializeField] private RankingDisplayEntry rankingDisplayEntrySelfFirstPlace;
        [SerializeField] private RankingDisplayEntry rankingDisplayEntrySelfSecondPlace;

        private Transform _ownEntryTransform;

        private void OnEnable()
        {
            StartCoroutine(RankingRequestHandler.GetRankings(Show));
            anchorButton.interactable = SignInManager.IsPlayerLoggedIn;
        }

        private void Show(RankingData rankingData)
        {
            // var rankingDisplayEntriesCount = rankingEntriesPanel.childCount;
            var rankingEntriesCount = rankingData.entries.Length;
            
            // // If there are more ranking display entries than actual entries, destroy unnecessary entries
            // if (rankingDisplayEntriesCount > rankingEntriesCount)
            // {
            //     for (var i = rankingEntriesCount; i < rankingDisplayEntriesCount; i++)
            //     {
            //         Destroy(rankingEntriesPanel.GetChild(i));
            //     }
            // }

            foreach (Transform child in rankingEntriesPanel)
            {
                Destroy(child.gameObject);
            }

            for (var position = 0; position < rankingEntriesCount; position++)
            {
                var entry = rankingData.entries[position];
                var ownEntry = (SignInManager.IsPlayerLoggedIn && entry.playerId == SignInManager.PlayerStatus.id);

                var newEntry = position switch
                {
                    0 => !ownEntry ? rankingDisplayEntryFirstPlace : rankingDisplayEntrySelfFirstPlace,
                    1 => !ownEntry ? rankingDisplayEntrySecondPlace : rankingDisplayEntrySelfSecondPlace,
                    _ => !ownEntry ? rankingDisplayEntry : rankingDisplayEntrySelf
                };

                // var rankingDisplayEntry = position < rankingDisplayEntriesCount ? 
                //     rankingEntriesPanel.GetChild(position).GetComponent<RankingEntryDisplay>() : 
                //     Instantiate(newEntry, rankingEntriesPanel);
                
                var rde = Instantiate(newEntry, rankingEntriesPanel);
                rde.Show(entry, position + 1);

                if (ownEntry)
                {
                    _ownEntryTransform = rde.transform;
                } 
            }
        }

        public void ScrollToPlayer()
        {
            if (_ownEntryTransform == null) return;

            var u = (Vector2) rankingScrollView.transform.InverseTransformPoint(rankingScrollView.content.position);
            var v = (Vector2) rankingScrollView.transform.InverseTransformPoint(_ownEntryTransform.position);
            rankingScrollView.content.anchoredPosition = (u - v) * Vector2.up;
        }
    }
}