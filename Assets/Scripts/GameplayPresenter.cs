using System.Collections.Generic;
using CardMatch.Models;
using UnityEngine;

namespace CardMatch.Gameplay
{
    public class GameplayPresenter : MonoBehaviour
    {
        public int rows = 2;
        public int columns = 2;
        public float spacing = .5f;
        public Transform cardsParent;
        
        private readonly List<CardPresenter> _allCards = new();
        private CardMatchEngine _cardMatchEngine;
        private Camera _mainCam;
        
        private void Start()
        {
            _mainCam = Camera.main;
            _cardMatchEngine = DependencyManager.Instance.cardMatchEngine;
            SetupBoard();

            foreach (var cardPresenter in _allCards)
            {
                cardPresenter.ShowThenHide(3);
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SetupBoard();
            }
        }

        private void SetupBoard()
        {
            // Destroy existing cards
            foreach (var card in _allCards)
                Destroy(card.gameObject);

            // Calculate total grid dimensions
            var cardSize = Vector2.zero;
            var gridWidth = 0f;
            var gridHeight = 0f;
            var type = 0;
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var newCard = DependencyManager.Instance.cardFactory.Create(
                        (CardType)type,
                        parent: cardsParent
                    );
                    newCard.CardClicked += _cardMatchEngine.CardClicked;
                    
                    if (i == 0 && j == 0)
                        cardSize = newCard.cardBackground.bounds.size;

                    // Calculate position for the card
                    var x = j * (cardSize.x + spacing);
                    var y = -i * (cardSize.y + spacing); // Negative for top-to-bottom row arrangement
                    var pos = new Vector3(x, y, 0);
                    newCard.transform.position = pos;

                    // Update grid dimensions
                    if (i == rows - 1)
                        gridHeight = Mathf.Abs(y) + cardSize.y;

                    if (j == columns - 1)
                        gridWidth = Mathf.Abs(x) + cardSize.x;

                    _allCards.Add(newCard);
                    if (_allCards.Count % 2 == 0)
                        type++;
                }
            }

            var offsetX = gridWidth / 2 - cardSize.x / 2;
            var offsetY = gridHeight / 2 - cardSize.y / 2;
            foreach (Transform card in cardsParent)
                card.position -= new Vector3(offsetX, -offsetY, 0);

            AdjustCamera(gridWidth, gridHeight);
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
    }
}