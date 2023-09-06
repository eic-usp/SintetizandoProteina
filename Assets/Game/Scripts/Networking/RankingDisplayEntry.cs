using Networking.DataStructs;
using TMPro;
using UnityEngine;

namespace Networking
{
    public class RankingDisplayEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI positionText;
        [SerializeField] private TextMeshProUGUI scoreText;

        public void Show(RankingEntry rankingEntry, int position)
        {
            nameText.text = rankingEntry.playerId;
            positionText.text = position.ToString();
            scoreText.text = rankingEntry.highScore.ToString();
            gameObject.SetActive(true);
        }
    }
}