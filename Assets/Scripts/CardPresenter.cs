using System;
using System.Collections;
using CardMatch.Models;
using UnityEngine;

namespace CardMatch.Gameplay
{
    public class CardPresenter : MonoBehaviour
    {
        public event Action<CardPresenter> CardFlippedByClick = delegate { };
        public event Action CardClicked = delegate { };
        public event Action<CardPresenter> CardDestroyed = delegate { };
        
        public CardType CardType { get; private set; }
     
        [SerializeField] public SpriteRenderer  cardBackground;
        [SerializeField] public SpriteRenderer  backOfTheCard;
        [SerializeField] private SpriteRenderer iconSprite;
        
        private bool _canBeClicked = true;
        
        private const float RotationHalfTime = .5f;
        private const float FlipTime = RotationHalfTime * .5f;
        private const float AnglePerFrame = 180 / RotationHalfTime;
        private const float MovingSpeed = 10;

        public void Setup(
            CardType t,
            Sprite s
        )
        {
            iconSprite.sprite = s;
            CardType = t;
            Flip(false);
        }

        public void ShowThenHide(float seconds)
        {
            StartCoroutine(ShowThenHideIE(seconds));
        }

        public void Hide()
        {
            StartCoroutine(FlipToFaceDown());
        }

        public IEnumerator GoTo(Vector3 targetPos)
        {
            //just to do the thing... its not safe :) 
            cardBackground.sortingOrder += 10;
            iconSprite.sortingOrder += 10;
            
            while (Vector3.Distance(transform.position,targetPos) > .01f)
            {
                transform.position = Vector3.Lerp(
                    transform.position, 
                    targetPos,
                    Time.deltaTime * MovingSpeed
                );
                yield return null;
            }
        }

        public void DestroyCard()
        {
            CardDestroyed.Invoke(this);
            Destroy(gameObject);
        }

        private void Flip(bool showFace)
        {
            iconSprite.sortingOrder = cardBackground.sortingOrder + (showFace ? 1 : -1);
            backOfTheCard.sortingOrder = cardBackground.sortingOrder + (showFace ? -1 : 1);
        }

        private void OnMouseDown()
        {
            if (_canBeClicked == false)
                return;

            if (_canBeClicked)
                StartCoroutine(FlipToFaceUp(fromClick: true));
        }

        //I would normally use DoTween to do these things...
        //but in the description, it have been mentioned not to use pre-built things...
        private IEnumerator ShowThenHideIE(float seconds)
        {
            yield return FlipToFaceUp(fromClick: false);
            yield return new WaitForSeconds(seconds);
            yield return FlipToFaceDown();
        }
        
        private IEnumerator FlipToFaceUp(bool fromClick)
        {
            CardClicked.Invoke();
            _canBeClicked = false;
            var flipped = false;
            var timer = 0f;
            while (timer < RotationHalfTime)
            {
                transform.Rotate(Vector3.up, Time.deltaTime * AnglePerFrame);
                timer += Time.deltaTime;
                if (!flipped && timer > FlipTime)
                {
                    Flip(true);
                    flipped = true;
                }
                
                yield return null;
            }
            transform.rotation = Quaternion.Euler(0,180,0);
            
            if (fromClick)
                CardFlippedByClick.Invoke(this);
        }
            
        private IEnumerator FlipToFaceDown()
        {
            var flipped = false;
            var timer = 0f;
            while (timer < RotationHalfTime)
            {
                transform.Rotate(Vector3.up, -Time.deltaTime * AnglePerFrame);
                timer += Time.deltaTime;
                if (!flipped && timer > FlipTime)
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