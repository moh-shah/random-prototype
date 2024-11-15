using System;
using System.Collections;
using System.Collections.Generic;
using CardMatch.Models;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CardMatch.Gameplay
{
    public class BoardPresenter : MonoBehaviour
    {
        public event Action OnBoardCleared = delegate { };
        public Transform cardsParent;

        private readonly List<CardPresenter> _allCards = new();
        private CardMatchEngine _cardMatchEngine;
        private AudioManager _audioManager;
        private CardFactory _cardFactory;
        private GameSettings _settings;
        private Camera _mainCam;

        public void Setup(
            CardMatchEngine matchEngine,
            CardFactory cardFactory,
            GameSettings settings,
            AudioManager audioManager
        )
        {
            _cardMatchEngine = matchEngine;
            _cardFactory = cardFactory;
            _settings = settings;
            _audioManager = audioManager;
        }

        public void PresentTheBoard()
        {
            _mainCam = Camera.main;
            StartCoroutine(SetupBoard());
        }

        private IEnumerator SetupBoard()
        {
            foreach (var card in _allCards)
                Destroy(card.gameObject);

            _allCards.Clear();

            // Calculate total grid dimensions
            var cardSize = Vector2.zero;
            var gridWidth = 0f;
            var gridHeight = 0f;
            var type = 0;
            for (var i = 0; i < _settings.rows; i++)
            {
                for (var j = 0; j < _settings.columns; j++)
                {
                    var newCard = _cardFactory.Create((CardType)type, parent: cardsParent);
                    newCard.CardFlippedByClick += _cardMatchEngine.CardFlippedByClick;
                    newCard.CardClicked += () => _audioManager.OnCardClicked();
                    newCard.CardDestroyed += CardDestroyed;

                    if (i == 0 && j == 0)
                        cardSize = newCard.cardBackground.bounds.size;

                    // Calculate position for the card
                    var x = j * (cardSize.x + _settings.spaceBtwnCards);
                    var y = -i * (cardSize.y + _settings.spaceBtwnCards); // Negative for top-to-bottom row arrangement
                    var pos = new Vector3(x, y, 0);
                    newCard.transform.position = pos;

                    // Update grid dimensions
                    if (i == _settings.rows - 1)
                        gridHeight = Mathf.Abs(y) + cardSize.y;

                    if (j == _settings.columns - 1)
                        gridWidth = Mathf.Abs(x) + cardSize.x;

                    _allCards.Add(newCard);
                    if (_allCards.Count % 2 == 0)
                        type++;

                    newCard.gameObject.SetActive(false);
                }
            }

            var offsetX = gridWidth / 2 - cardSize.x / 2;
            var offsetY = gridHeight / 2 - cardSize.y / 2;
            foreach (Transform card in cardsParent)
                card.position -= new Vector3(offsetX, -offsetY, 0);

            AdjustCamera(gridWidth, gridHeight);

            var wait = new WaitForSeconds(.05f);
            foreach (var cardPresenter in _allCards)
            {
                yield return wait;
                cardPresenter.gameObject.SetActive(true);
            }

            foreach (var cardPresenter in _allCards)
                cardPresenter.ShowThenHide(2);
        }

        private void AdjustCamera(float gridWidth, float gridHeight)
        {
            const float padding = 1f;

            // Calculate required orthographic size
            var horizontalSize = (gridWidth + padding) / 2 / _mainCam.aspect;
            var verticalSize = (gridHeight + padding) / 2;
            _mainCam.orthographicSize = Mathf.Max(horizontalSize, verticalSize);

            // Center the camera on the grid
            _mainCam.transform.position = new Vector3(0, 0, _mainCam.transform.position.z);
        }

        private void CardDestroyed(CardPresenter cardPresenter)
        {
            _allCards.Remove(cardPresenter);
            if (_allCards.Count == 0)
            {
                Debug.Log("board cleared!");
                OnBoardCleared.Invoke();
            }
        }
    }
}