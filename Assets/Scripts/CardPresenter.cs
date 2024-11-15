using System;
using CardMatch.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardMatch.Gameplay
{
    public class CardPresenter : MonoBehaviour 
    {
        public CardType CardType { get; private set; }
     
        [SerializeField] public SpriteRenderer cardBackground;
        [SerializeField] private SpriteRenderer iconSprite;
        
        private bool _frontFace;
        
        public void Setup(CardType t)
        {
            iconSprite.sprite = DependencyManager.Instance.spritesProvider.GetSpriteFor(t);
            CardType = t;
        }

        public void Flip()
        {
            _frontFace = !_frontFace;
            Debug.Log($"flipping to {_frontFace}");
            //do animation
            iconSprite.sortingOrder = cardBackground.sortingOrder + (_frontFace ? 1 : -1);
        }

        private void OnMouseDown()
        {
            Debug.Log("clicked");
            Flip();
        }
    }
}