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
        
        private void Start()
        {
            ArrangeCards();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ArrangeCards();
            }
        }

        private void ArrangeCards()
        {
            // Destroy existing cards
            foreach (Transform card in cardsParent)
                Destroy(card.gameObject);

            // Calculate total grid dimensions
            var cardSize = Vector2.zero;
            var gridWidth = 0f;
            var gridHeight = 0f;
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var newCard = DependencyManager.Instance.cardFactory.Create(CardType.Apple, parent: cardsParent);
                    
                    if (i == 0 && j == 0)
                    {
                        // Fetch card size from the first card created
                        cardSize = newCard.cardBackground.bounds.size;
                    }

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
            var mainCamera = Camera.main;
            
            // Calculate required orthographic size
            var horizontalSize = (gridWidth + padding) / 2 / mainCamera.aspect;
            var verticalSize = (gridHeight + padding) / 2;
            mainCamera.orthographicSize = Mathf.Max(horizontalSize, verticalSize);

            // Center the camera on the grid
            mainCamera.transform.position = new Vector3(0, 0, mainCamera.transform.position.z);
        }
    }
}