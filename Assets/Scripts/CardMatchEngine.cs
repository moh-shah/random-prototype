using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardMatch.Gameplay
{
    public class CardMatchEngine : MonoBehaviour
    {
        private CardPresenter _lastClickedCard;
        private Queue<(CardPresenter c1, CardPresenter c2)> _processQ = new();
        private (CardPresenter c1, CardPresenter c2) _clickedBuffer;
        private bool _isPresenting;
        
        public void CardClicked(CardPresenter clickedCard)
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
                        yield return new WaitForSeconds(2);
                        Destroy(cardPair.c1.gameObject);
                        Destroy(cardPair.c2.gameObject);
                    }
                    else
                    {
                        Debug.Log("No Match!");
                        cardPair.c1.Hide();
                        cardPair.c2.Hide();
                    }

                    _isPresenting = false;
                }
            }
        }
    }
}