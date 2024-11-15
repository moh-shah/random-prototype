using System;
using System.Collections;
using CardMatch.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardMatch.Gameplay
{
    public class CardPresenter : MonoBehaviour 
    {
        public CardType CardType { get; private set; }
     
        [SerializeField] public SpriteRenderer cardBackground;
        [SerializeField] public SpriteRenderer backOfTheCard;
        [SerializeField] private SpriteRenderer iconSprite;
        
        private bool _canBeClicked = true;
        private bool _facedUp;
        
        public void Setup(CardType t)
        {
            iconSprite.sprite = DependencyManager.Instance.spritesProvider.GetSpriteFor(t);
            CardType = t;
            Flip(false);
        }

        public void Flip(bool showFace)
        {
            _facedUp = showFace;
            iconSprite.sortingOrder = cardBackground.sortingOrder + (showFace ? 1 : -1);
            backOfTheCard.sortingOrder = cardBackground.sortingOrder + (showFace ? -1 : 1);
        }

        private void OnMouseDown()
        {
            if (_canBeClicked == false)
                return;    
            
            StartCoroutine(Show(2));
        }

        //I would normally use DoTween to do these things... but in the description, it have been mentioned not to use pre-built things...
        private IEnumerator Show(float seconds)
        {
            const float rotationHalfTime = .5f;
            const float flipTime = rotationHalfTime * .5f;
            const float anglePerFrame = 180 / rotationHalfTime;
            var flipped = false;
            _canBeClicked = false;
            var timer = 0f;
            while (timer < rotationHalfTime)
            {
                transform.Rotate(Vector3.up, Time.deltaTime * anglePerFrame);
                timer += Time.deltaTime;
                if (!flipped && timer > flipTime)
                {
                    Flip(true);
                    flipped = true;
                }
                
                yield return null;
            }
            transform.rotation = Quaternion.Euler(0,180,0);
            
            yield return new WaitForSeconds(seconds);

            flipped = false;
            timer = 0f;
            while (timer < rotationHalfTime)
            {
                transform.Rotate(Vector3.up, -Time.deltaTime * anglePerFrame);
                timer += Time.deltaTime;
                if (!flipped && timer > flipTime)
                {
                    Flip(false);
                    flipped = true;
                }
                
                yield return null;
            }
            transform.rotation = Quaternion.Euler(0,0,0);
            _canBeClicked = true;
        }
    }
}