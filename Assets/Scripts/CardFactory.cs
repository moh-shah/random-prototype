using System;
using CardMatch.Models;
using UnityEngine;

namespace CardMatch.Gameplay
{
    public class CardFactory : MonoBehaviour
    {
        [SerializeField] private CardPresenter cardPresenter;
        private SpritesProvider _spritesProvider;
        public void Setup(SpritesProvider spritesProvider)
        {
            _spritesProvider = spritesProvider;
        }

        public CardPresenter Create(
            CardType cardType,
            Vector3 position = default,
            Transform parent = null
        )
        {
            var newCard = Instantiate(cardPresenter, parent);
            newCard.Setup(cardType, _spritesProvider.GetSpriteFor(cardType));
            newCard.transform.position = position;
            return newCard;
        }
    }
}