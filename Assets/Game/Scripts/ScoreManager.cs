using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Networking.DataStructs;
using Networking.RequestHandlers;

public class ScoreManager : MonoBehaviour
{
    [SerializeField, Min(0)] private int maxScore;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private RetryMenu retryMenu;

    public static ScoreManager Instance { get; private set; }
    public int Score { get; private set; }
    
    [field: SerializeField] public int DefaultPenaltyRequirement { get; set; } = 3;
    [field: SerializeField] public int DefaultBonusRequirement { get; set; } = 3;
    [field: SerializeField] public int DefaultTimeBonusRequirement { get; set; } = 15;
    [field: SerializeField] public int DefaultTimePenaltyRequirement { get; set; } = 30;
    
    public enum ScoreContext
    {
        InMissionHit,
        InMissionHitBonus,
        InMissionMiss,
        InMissionMissPenalty,
        MissionCompleted
    }

    // private static readonly Dictionary<ScoreContext, int> ScoreTable = new()
    // {
    //     { ScoreContext.InMissionHit, 5 },
    //     { ScoreContext.InMissionHitBonus, 15 },
    //     { ScoreContext.InMissionMiss, -5 },
    //     { ScoreContext.InMissionMissPenalty, -15 },
    //     { ScoreContext.MissionCompleted, 20 }
    // };

    [System.Serializable]
    private struct ScoreTableEntry
    {
        public ScoreContext context;
        public int score;
    }

    [SerializeField] private ScoreTableEntry[] scoreTable;

    private int ScoreTable(ScoreContext context) => System.Array.Find(scoreTable, entry => entry.context == context).score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    public void UpdateScore(ScoreContext context)
    {
        var s = Score;
        var ns = ScoreTable(context);
        Score = Mathf.Clamp(Score + ScoreTable(context), 0, maxScore != 0 ? maxScore : int.MaxValue);
        Debug.Log($"(Score) {s} -> {Score} [{(Mathf.Sign(ns) == 1 ? "+" : "")}{ns}]");
    }

    public void FinishMatch()
    {
        StartCoroutine(FinishMatchEnumerator());
    }

    private IEnumerator FinishMatchEnumerator()
    {
        yield return MatchRequestHandler.CreateMatch(
            new MatchData { score = Score },
            _ => gameOverPanel.gameObject.SetActive(true),
            OnFailure
            );
    }

    private void OnFailure(UnityWebRequest req)
    {
        Debug.Log(req.responseCode);
            
        switch (req.responseCode)
        {
            case 401:
                retryMenu.SessionExpiredInGame();
                break;
            default:
                retryMenu.InternetConnectionLost(FinishMatchEnumerator());
                break;
        }
    }
}