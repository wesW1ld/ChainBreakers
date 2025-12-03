using System.Collections;
using System.Collections.Generic;
using ChainBreakers;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayList : MonoBehaviour
{
    private List<Card> PlayedCards = new List<Card>();
    private List<GameObject> CardObjects = new List<GameObject>();
    private int CardLimit = 3;
    private int BaseCardLimit = 3;

    public int enemyNum = 0;

    public void Push(Card card, GameObject cardO) //hand is gonna move cards, then when turn is over, push them all in order
    {
        if(PlayedCards.Count == CardLimit)
        {
            Debug.Log("Max number of cards played");
            return;
        }
        PlayedCards.Add(card);
        CardObjects.Add(cardO);
    }

    public void Pop(Card card, GameObject cardO)
    {
        PlayedCards.Remove(card);
        CardObjects.Remove(cardO);
    }

    public Card[] SeeStack() //see current stack with top being index 0
    {
        Card[] ret = PlayedCards.ToArray();
        return ret;
    }

    public int ClearList()
    {
        int ret = CardObjects.Count;

        // Iterate backwards to avoid collection modification issues
        for (int i = CardObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = CardObjects[i];
            if (obj != null)
            {
                Destroy(obj);
            }
            CardObjects.RemoveAt(i); // safely remove from list
        }

        PlayedCards.Clear(); // optional if you want to clear card data too

        CardLimit = BaseCardLimit;

        if(HandManager.instance.HandSizeDif() < ret)
        {
            return HandManager.instance.HandSizeDif();
        }
        else
        {
            return ret;
        }
        
    }

    public int GetSize()
    {
        return PlayedCards.Count;
    }

    public bool IsFull()
    {
        return CardObjects.Count >= CardLimit;
    }
    
    //singleton stuff
    public static PlayList instance;
    public static PlayList Instance
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
            Debug.Log("playermanager awakened");
            //DontDestroyOnLoad(this); //stay over scene
        }

    }

    public void PlayAllCards()
    {

        Card[] items = SeeStack();
        Card.CardType prev = Card.CardType.Attack;//default, but gets changed before use
        int streak = -1;

        for (int i = items.Length - 1; i >= 0; i--)
        {
            Card card = items[i];
            if(streak == -1)
            {
                prev = card.cardType;
            }
            if(card.cardType == prev)
            {
                streak += 1;
            }
            else
            {
                streak = 0;
            }

            if(card.cardType == Card.CardType.Attack)
            {
                float mult = 1;
                if(PlayerManager.instance.MightCheck() && !PlayerManager.instance.WeakendCheck())
                {
                    mult = 1.5f;
                }
                else if(PlayerManager.instance.WeakendCheck() && PlayerManager.instance.MightCheck())
                {
                    mult = .75f;
                }
                for(int j = 0; j < streak; j++)
                {
                    mult += .1f;
                }
                EnemyManager.Instance.DealDamage((int)(mult * Random.Range(card.min, card.max + 1)), enemyNum);
            }
            else if(card.cardType == Card.CardType.Defend)
            {
                float mult = 1f;
                for(int j = 0; j < streak; j++)
                {
                    mult += .1f;
                }
                PlayerManager.instance.AddShield((int)(mult * Random.Range(card.min, card.max + 1)));
            }

            //depends on the card, not including status effects done below
            if(card.cardName == "Medical Training")
            {
                float mult = 1f;
                for(int j = 0; j < streak; j++)
                {
                    mult += .1f;
                }
                PlayerManager.instance.Heal((int)(mult * Random.Range(card.min, card.max + 1)));
            }
            else if(card.cardName == "Ask Chat")
            {
                DeckManager.instance.DrawCard();
                DeckManager.instance.DrawCard();
            }
            else if(card.cardName == "Bag Boy")
            {
                CardLimit++;
            }
            else if(card.cardName == "Flame Shot")
            {
                EnemyManager.Instance.DealDamage((int)(Random.Range(card.min, card.max + 1)), enemyNum);
                EnemyManager.Instance.DealDamage((int)(Random.Range(card.min, card.max + 1)), enemyNum + 1);
                EnemyManager.Instance.DealDamage((int)(Random.Range(card.min, card.max + 1)), enemyNum + 2);
            }
            else if(card.cardName == "Healing Aura")
            {
                float mult = 1f;
                for(int j = 0; j < streak; j++)
                {
                    mult += .1f;
                }
                PlayerManager.instance.Heal((int)(mult * Random.Range(card.min, card.max + 1)));
                PlayerManager.instance.AddShield((int)(mult * Random.Range(card.minTurn, card.maxTurn + 1)));
            }
            else if(card.cardName == "Stim Shot")
            {
                PlayerManager.instance.Buff(Card.StatusEffect.might, Random.Range(card.minTurn + 1, card.maxTurn + 1));
            }
            else if(card.cardName == "All Of The Lights")
            {
                BaseCardLimit += Random.Range(card.minTurn, card.maxTurn + 1);
            }
            else if(card.cardName == "Dealer's Choice")
            {
                DeckManager.instance.DrawCard();
                DeckManager.instance.DrawCard();
                DeckManager.instance.DrawCard();
                BaseCardLimit--;
            }
            else if(card.cardName == "Lightning Cast")
            {
                EnemyManager.Instance.ApplyStatus(Card.StatusEffect.Shocked, Random.Range(card.minTurn + 1, card.maxTurn + 1), enemyNum);
                EnemyManager.Instance.ApplyStatus(Card.StatusEffect.Shocked, Random.Range(card.minTurn + 1, card.maxTurn + 1), enemyNum + 1);
                EnemyManager.Instance.ApplyStatus(Card.StatusEffect.Shocked, Random.Range(card.minTurn + 1, card.maxTurn + 1), enemyNum + 2);
                PlayerManager.instance.TakeDamage(Random.Range(card.min, card.max + 1));
            }

            foreach(Card.StatusEffect sta in card.statusEffects)
            {
                if(sta == Card.StatusEffect.might || sta == Card.StatusEffect.poise || sta == Card.StatusEffect.Regenerative)
                {
                    PlayerManager.instance.Buff(sta, Random.Range(card.minTurn + 1, card.maxTurn + 1));
                }
                else
                {
                    EnemyManager.Instance.ApplyStatus(sta, Random.Range(card.minTurn + 1, card.maxTurn + 1), enemyNum);
                }
                
            }

            prev = card.cardType;
        }
    } 
}
