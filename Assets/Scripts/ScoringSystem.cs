using System;
using CardMatch.Models;
using UnityEngine;

namespace CardMatch.Gameplay
{
    public class ScoringSystem
    {
        public event Action<int> ScoreChanged = delegate {  };
        public event Action<int> ComboChanged = delegate {  };

        public int Score
        {
            get;
            private set;
        }
        
        private int _combo;
        
        public ScoringSystem(CardMatchEngine matchEngine, GameSettings settings)
        {
            matchEngine.OnMatch += delegate
            {
                var amp = _combo == 0 ? 1 : _combo * settings.comboScoreAmplifier;
                Score += settings.scorePerMatch * amp;
                _combo++;
                ScoreChanged.Invoke(Score);
                ComboChanged.Invoke(_combo);
                Debug.Log($"score: {Score} | combo: {_combo}");
            };
            
            matchEngine.OnMisMatch += delegate
            {
                _combo = 0;
                ComboChanged.Invoke(_combo);
                Debug.Log($"score: {Score} | combo: {_combo}");
            };
        }
    }
}