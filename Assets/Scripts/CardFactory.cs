using CardMatch.Models;
using UnityEngine;

namespace CardMatch.Gameplay
{
    public class CardFactory : MonoBehaviour
    {
        [SerializeField] private CardPresenter cardPresenter;
        
        public CardPresenter Create(CardType cardType, Vector3 position = default, Transform parent = null)
        {
            var newCard = Instantiate(cardPresenter, parent);
            newCard.Setup(cardType);
            newCard.transform.position = position;
            return newCard;
        }
    }
}