using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardMatch.Gameplay
{
    public class CardMatchEngine : MonoBehaviour
    {
        public event Action OnMatch = delegate { };
        public event Action OnMisMatch = delegate { };
        
        private CardPresenter _lastClickedCard;
        private Queue<(CardPresenter c1, CardPresenter c2)> _processQ = new();
        private (CardPresenter c1, CardPresenter c2) _clickedBuffer;
        private bool _isPresenting;
        
        public void CardFlippedByClick(CardPresenter clickedCard)
        {
            if (_clickedBuffer.c1 == null) 
                _clickedBuffer.c1 = clickedCard;
            else if (_clickedBuffer.c2 == null)
                _clickedBuffer.c2 = clickedCard;

            if (_clickedBuffer.c1 != null && _clickedBuffer.c2!=null)
            {
                _processQ.Enqueue(_clickedBuffer);
                _clickedBuffer.c1 = null;
                _clickedBuffer.c2 = null;
            }
        }

        private void Start()
        {
            StartCoroutine(CardProcessor());
        }

        private void OnDestroy()
        {
            StopCoroutine(CardProcessor());
        }

        private IEnumerator CardProcessor()
        {
            while (true)
            {
                yield return null;
                if (_isPresenting)
                    continue;

                if (_processQ.Count > 0)
                {
                    _isPresenting = true;
                    var cardPair = _processQ.Dequeue();
                    if (cardPair.c1.CardType == cardPair.c2.CardType)
                    {
                        Debug.Log("Match!");
                        var goTos = new[]
                        {
                            StartCoroutine(cardPair.c1.GoTo(Vector3.zero)),
                            StartCoroutine(cardPair.c2.GoTo(Vector3.zero))
                        };
                        
                        foreach (var goTo in goTos)
                            yield return goTo;
                        
                        OnMatch.Invoke();
                        cardPair.c1.DestroyCard();
                        cardPair.c2.DestroyCard();
                    }
                    else
                    {
                        Debug.Log("No Match!");
                        cardPair.c1.Hide();
                        cardPair.c2.Hide();
                        OnMisMatch.Invoke();
                    }

                    _isPresenting = false;
                }
            }
        }
    }
}