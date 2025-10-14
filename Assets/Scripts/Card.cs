using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChainBreakers
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject
    {
        public string cardName;
        public Sprite artwork;
        public int min;
        public int max;
        public int minTurn;
        public int maxTurn;
        public List<CardType> cardTypes;
        public List<StatusEffect> statusEffects;
        public string description;
        internal object transform;

        public enum CardType
        {
            Attack,
            Defend,
            Status,
            Special
        }
        public enum StatusEffect
        {
            Dazed,
            Enraged,
            Emboldened,
            Shocked,
            Rage,
            Cloaked,
            Blur,
            Binded,
            Regenerative 
        }
    }
}
