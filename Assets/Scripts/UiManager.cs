using TMPro;
using UnityEngine;

namespace CardMatch.Gameplay
{
    public class UiManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI comboText;
        public TextMeshProUGUI bestScore;
        
        public void Setup(ScoringSystem scoringSystem, int currentBestScore)
        {
            scoringSystem.ScoreChanged += score => { scoreText.text = $"Score: {score}"; };
            scoringSystem.ComboChanged += combo => { comboText.text = $"Combo: {combo}"; };
            bestScore.text = $"BestScore: {currentBestScore}";
        }
    }
}