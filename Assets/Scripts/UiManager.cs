using System;
using TMPro;
using UnityEngine;

namespace CardMatch.Gameplay
{
    public class UiManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI comboText;
        public TextMeshProUGUI bestScore;
        
        public TextMeshProUGUI scoreTextInWinPanel;
        public GameObject startUpPanel;
        public GameObject winPanel;
        
        public void Setup(ScoringSystem scoringSystem, int currentBestScore)
        {
            scoringSystem.ScoreChanged += score => { scoreText.text = $"Score: {score}"; };
            scoringSystem.ComboChanged += combo => { comboText.text = $"Combo: {combo}"; };
            bestScore.text = $"BestScore: {currentBestScore}";
        }

        public void OnGameEnded()
        {
            winPanel.SetActive(true);
            scoreTextInWinPanel.text = scoreText.text;
        }
        
        private void Start()
        {
            startUpPanel.SetActive(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (startUpPanel.activeInHierarchy)
                {
                    startUpPanel.SetActive(false);
                }
            }
        }
    }
}