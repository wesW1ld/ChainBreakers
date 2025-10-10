using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;

    public static DeckManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameManager is Null");
            }
            return instance;
        }

    }
    private void Awake()
    {

        if (instance)
        {
            Debug.LogError("GameManager is already in the scene");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

    }


    public enum CardType
    {
        Atk,
        Def,
        Status,
        Spec
    }

    public enum CardName
    {
        ShortStrike,
        HammerTime,
        HunkerDown,
        RageBait,
        ScoutScan,
        MedicalTraining,
        TaserSpear,
        FlameShot,
        StimShot,
        HealingAura,
        IronArmor,
        FullyCharged,
        NowThisIsAKnife,
        BagBoy,
        MuscleUp,
        AskChat,
        NowYouDont,
        LightningCast,
        GrubUp,
        FromTheTop,
        InTheBushes,
        NaturesGlitch,
        AllOfTheLights,
        PunjiPit,
        DealersChoice,
        NanoBark,
        GeneticallyModifiedBanana
    }

    public enum StatusEffect
    {
        None,
        Dazed,
        Enraged,
        Emboldened,
        Shocked,
        Rage,
        Cloaked,
        Blur,
        Binded,
        Regenerative
        ,Poise
    }

    public struct Card
    {
        public CardName name;
        public CardType type;
        public int lower;
        public int higher;
        public int turnLower;
        public int turnHigher;
        public int uses;                     //for cards that can be used multiple times
        public Sprite artwork;
        public StatusEffect statusEffect;
        public string description;
        public bool dropped;                    //card can only be used once


        //Cards that are static in their output
        public Card(CardName name, CardType type)
        {
            this.name = name;
            this.type = type;
            this.lower = 0;
            this.higher = 0;
            this.turnLower = 0;
            this.turnHigher = 0;
            this.uses = 0;
            this.artwork = null;
            this.statusEffect = StatusEffect.None;
            this.description = "";
            this.dropped = false;
        }

        //Cards with variable output amounts 
        public Card(CardName name, CardType type, int lower, int upper, StatusEffect statusEffect)
        {
            this.name = name;
            this.type = type;
            this.lower = lower;
            this.higher = upper;
            this.turnLower = 0;
            this.turnHigher = 0;
            this.uses = 0;
            this.artwork = null;
            this.statusEffect = statusEffect;
            this.description = "";
            this.dropped = false;
        }

        //Cards with an additional variable for various effects 
        public Card(CardName name, CardType type, int lower, int upper, int turnLower, int turnHigher, StatusEffect statusEffect)
        {
            this.name = name;
            this.type = type;
            this.lower = lower;
            this.higher = upper;
            this.turnLower = turnLower;
            this.turnHigher = turnHigher;
            this.uses = 0;
            this.artwork = null;
            this.statusEffect = statusEffect;
            this.description = "";
            this.dropped = false;
        }

        //Cards will an additional amount of uses
        public Card(CardName name, CardType type, int lower, int upper, int turnLower, int turnHigher, StatusEffect statusEffect, int uses)
        {
            this.name = name;
            this.type = type;
            this.lower = lower;
            this.higher = upper;
            this.turnLower = turnLower;
            this.turnHigher = turnHigher;
            this.statusEffect = statusEffect;
            this.uses = uses;
            this.artwork = null;
            this.description = "";
            this.dropped = false;
        }
    }

    public void UseCard(Card card)
    {
        switch (card.name)
        {
            case CardName.ShortStrike:
                //based on score do card.lower to card.higher damage
                card.description = $"Does {card.lower} to {card.higher} damage";
                break;

            case CardName.HunkerDown:
                //reduce damage by half
                card.description = $"Adds {card.lower} to {card.higher} shield";
                break;

            case CardName.HammerTime:
                //daze enemy for 1 turn
                card.description = $"Does {card.lower} to {card.higher} damage and Dazes the enemy for {card.turnLower} to {card.turnHigher} turns";
                break;
            
            case CardName.MedicalTraining:
                card.description = $"Heals {card.lower} to {card.higher} amount of health.";
                break;
            
            case CardName.RageBait:
                //enemy gets enraged
                card.description = $"Emboldens and Enrages the enemy for 1 turn. Is Dropped after use.";
                card.dropped = true;
                break;

            case CardName.ScoutScan:
                //Still working on this one
                card.description = $"";
                break;

            case CardName.TaserSpear:
                //Deal damage and inflict shocked
                card.description = $"Damage the enemy for {card.lower} to {card.higher} damage and inflict Shocked them for {card.turnLower} to {card.turnHigher} turns.";
                break;

            case CardName.FlameShot:
                //Deal damage x amount of times based on number of enemies
                card.description = $"Damage the enemy for {card.lower} to {card.higher} damage per each enemy in combat.";
                break;

            case CardName.StimShot:
                //Heal and gain a percent chance to get rage buff
                card.description = $"Heals the player for {card.lower} to {card.higher} plus get a 50% chance to get Rage Buff for 1 turn.";
                break;

            case CardName.HealingAura:
                //Powerful healing
                card.description = $"Heals the player for {card.lower} to {card.higher}% of their max health. Is Dropped after use.";
                card.dropped = true;
                break;

            case CardName.IronArmor:
                //Shield + buff
                card.description = $"Adds {card.lower} to {card.higher} shield and gives {card.turnLower} to {card.turnHigher} poise.";
                break;

            case CardName.FullyCharged:
                //Choose between atk or def buff
                card.description= $"On a perfect score only, Choose between gaining +1 might or poise at the start of your turn for the rest of the combat.Is Dropped after use.";
                card.dropped = true;
                break;

            case CardName.NowThisIsAKnife:
                //Still working on this one
                card.description = $"";
                break;

            case CardName.BagBoy:
                //Adds cards and shield
                card.description = $"Adds {card.lower} to {card.higher} shield and will draw an extra card next turn plus increase play card slot by 1.";
                break;

            case CardName.MuscleUp:
                //Temp buff to might
                card.description = $"Gain 3 might for 1 turn.";
                break;
            case CardName.AskChat:
                //Add cards to the hand
                card.description = $"Add up to 2 cards from a set of 3 random cards to your hand.";
                break;

            case CardName.NowYouDont:
                //Become cloaked or deal more damage
                card.description = $"On a perfect score only, Choose between becoming Cloaked for 1 turn.Regardless, increase damage out by {card.lower} to {card.higher}% for {card.turnLower} to {card.turnHigher} turns. Is Dropped after use.";
                card.dropped = true;
                break;

            case CardName.LightningCast:
                //Damage + cards back to hand
                card.description = $"Deal {card.lower} to {card.higher} damage to all enemies and add {card.turnLower} to {card.turnHigher} cards from your discard pile to your hand.";
                break;

            case CardName.GrubUp:
                //Heal and gain cards next turn
                card.description = $"Heal {card.lower} to {card.higher} health and gain {card.turnLower} to {card.turnHigher} cards next turn.";
                break;

            case CardName.FromTheTop:
                //Damaege per cards in hand
                card.description = $"Deal {card.lower} to {card.higher} damage for each card in your hand.";
                break;

            case CardName.InTheBushes:
                //Shield + blur
                card.description = $"Adds {card.lower} to {card.higher} shield and gives {card.turnLower} to {card.turnHigher} blur.";
                break;

            case CardName.NaturesGlitch:
                //Binds the enemy
                card.description = $"Binds the enemy for {card.turnLower} to {card.turnHigher} turns. On a perfect score, the enemy will miss their next attack. Is Dropped after use.";
                card.dropped = true;
                break;

            case CardName.AllOfTheLights:
                //Increase play card limit
                card.description = $"Increase play card limit by {card.lower} to {card.higher} for the rest of the combat. Is Dropped after use.";
                card.dropped = true;                
                break;

            case CardName.PunjiPit:
                //Still working on this one
                card.description = $"";
                break;
            
            case CardName.DealersChoice:
                //Adding random cards to hand only 2 uses
                if (card.uses != 0)
                {
                    card.description = $"Add {card.lower} to {card.higher} random cards to your hand. Is Dropped after use 2 uses.";
                    card.uses--;
                }
                else
                {
                    card.dropped = true;
                }
                break;
            
            case CardName.NanoBark:
                //Gain regenerative
                card.description = $"Gain Regenerative for {card.turnLower} to {card.turnHigher} turns.";
                break;
            
            case CardName.GeneticallyModifiedBanana:
                //Gain random buffs
                card.description = $"Gain {card.lower}-{card.higher} random buffs for {card.turnLower} to {card.turnHigher} turns. Is Dropped after use.";
                card.dropped = true;
                break;

             default:
            Debug.Log("Card not found");
            break;
    }
}

    public List<Card> deck = new List<Card>();

    void Start()
    {
        MakeDeck();
    }

    private void MakeDeck()
    {
        for (int i = 0; i < 3; i++)
        {
            deck.Add(new Card(CardName.ShortStrike, CardType.Atk, 5, 10, StatusEffect.None));
            deck.Add(new Card(CardName.HunkerDown, CardType.Def, 2, 5, StatusEffect.None));
        }
        for (int i = 0; i < 2; i++)
        {
            deck.Add(new Card(CardName.HammerTime, CardType.Atk, 3,7, 1, 2, StatusEffect.Dazed));
            deck.Add(new Card(CardName.MedicalTraining, CardType.Def, 2, 5, StatusEffect.None));

        }
        deck.Add(new Card(CardName.RageBait, CardType.Status, 0, 0, StatusEffect.Enraged));
        deck.Add(new Card(CardName.ScoutScan, CardType.Spec));
    }

    private void Area1Pool()
    {
        deck.Add(new Card(CardName.TaserSpear, CardType.Atk, 3, 7, 1, 2, StatusEffect.Shocked));
        deck.Add(new Card(CardName.FlameShot, CardType.Atk, 2, 5, StatusEffect.None));
        deck.Add(new Card(CardName.StimShot, CardType.Def, 7, 14, StatusEffect.Rage));
        deck.Add(new Card(CardName.HealingAura, CardType.Def, 7, 13, StatusEffect.None));
        deck.Add(new Card(CardName.IronArmor, CardType.Def, 5, 8, 1, 2, StatusEffect.Poise));
        deck.Add(new Card(CardName.FullyCharged, CardType.Spec, 0, 0, StatusEffect.None));
        deck.Add(new Card(CardName.MuscleUp, CardType.Status, 0, 0, StatusEffect.None));
    }

    private void Area1ShopPool()
    {
        deck.Add(new Card(CardName.BagBoy, CardType.Def, 0, 0, StatusEffect.None));
        deck.Add(new Card(CardName.NowThisIsAKnife, CardType.Atk, 7, 10, StatusEffect.None));
        deck.Add(new Card(CardName.AskChat, CardType.Spec, 0, 0, StatusEffect.None));
    }

    private void Area2Pool()
    {
        
    }
}
