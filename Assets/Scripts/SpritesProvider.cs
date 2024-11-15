using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMatch.Models
{
    [CreateAssetMenu(fileName = "SpriteProvider", menuName = "CardMatch/SpriteProvider")]
    public class SpritesProvider : ScriptableObject
    {
        public List<CardModel> fruitSprites;

        public Sprite GetSpriteFor(CardType cardType)
        {
            return fruitSprites.Find(c => c.cardType == cardType)?.sprite;
        }
    }

    [Serializable]
    public class CardModel
    {
        public CardType cardType;
        public Sprite sprite;
    }

    public enum CardType
    {
        Apple,
        Banana,
        Blackberry,
        BlueBerry,
        Cherry,
        Coconut,
        Eggplant,
        Grapes,
        Kiwi,
        Lemon,
        Olive,
        Orange,
        Pear,
        Pineapple,
        Plum,
        Raspberry,
        Strawberry,
        Tomato,
        Watermelon,
        WatermelonSliced,
    }
}