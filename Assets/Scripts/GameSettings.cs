using System;
using UnityEngine;

namespace CardMatch.Models
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "CardMatch/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public int rows;
        public int columns;
        public float spaceBtwnCards;
        public int scorePerMatch;
        public int comboScoreAmplifier;
        
        [Space]
        public AudioClip onCardClicked;
        public AudioClip onCardsMatch;
        public AudioClip onCardsMisMatch;
        public AudioClip onBoardCleared;

        private const int MaxCardsCount = 40;
        
        private void OnValidate()
        {
            if (rows * columns % 2 != 0)
            {
                rows++;
                Debug.LogError($"[GameSettings]: rows * columns must be dividable by 2.... increasing row count to fix it");
            }

            while (rows * columns > MaxCardsCount)
            {
                if (rows > columns)
                    rows--;
                else
                    columns--;
                Debug.LogError($"[GameSettings]: rows * columns should not be more than {MaxCardsCount}");
            }
            
            if (rows * columns % 2 != 0)
            {
                rows--;
                Debug.LogError($"[GameSettings]: rows * columns must be dividable by 2.... decreeasing row count to fix it");
            }
        }
    }
}