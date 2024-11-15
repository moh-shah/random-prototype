using UnityEngine;

namespace CardMatch.Gameplay
{
    public class PersistentDataManager
    {
        private const string BestScoreKey = "bestScore";

        private ScoringSystem _scoringSystem;
        
        public PersistentDataManager(ScoringSystem scoringSystem)
        {
            _scoringSystem = scoringSystem;
        }

        public void OnGameEnded()
        {
            if (BestScore()  < _scoringSystem.Score)
            {
                PlayerPrefs.SetInt(BestScoreKey, _scoringSystem.Score);
            }
        }

        public int BestScore() => PlayerPrefs.GetInt(BestScoreKey, 0);
    }
}