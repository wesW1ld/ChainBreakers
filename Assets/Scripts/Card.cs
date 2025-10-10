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
        public int minDam;
        public int maxDam;
        public List<CardType> cardTypes;
        public List<StatusEffect> statusEffects;
        public string description;


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
