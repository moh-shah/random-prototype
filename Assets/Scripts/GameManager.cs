using CardMatch.Models;
using UnityEngine;

namespace CardMatch.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public BoardPresenter boardPresenter;
        public CardMatchEngine cardMatchEngine;
        public CardFactory cardFactory;
        public UiManager uiManager;
        public AudioManager audioManager;
        
        public SpritesProvider spritesProvider;
        public GameSettings settings;

        private ScoringSystem _scoringSystem;
        private PersistentDataManager _persistentDataManager;
        
        private void Awake()
        {
            _scoringSystem = new ScoringSystem(cardMatchEngine, settings);
            _persistentDataManager = new PersistentDataManager(_scoringSystem);
            cardFactory.Setup(spritesProvider);
            uiManager.Setup(_scoringSystem, _persistentDataManager.BestScore());
            audioManager.Setup(settings);
            boardPresenter.Setup(cardMatchEngine, cardFactory, settings, audioManager);
            boardPresenter.OnBoardCleared += _persistentDataManager.OnGameEnded;
            boardPresenter.OnBoardCleared += audioManager.OnGameEnded;
            boardPresenter.OnBoardCleared += uiManager.OnGameEnded;
            cardMatchEngine.OnMatch += audioManager.OnMatch;
            cardMatchEngine.OnMisMatch += audioManager.OnMisMatch;
        }

        private void Start()
        {
            boardPresenter.PresentTheBoard();
        }
    }
}