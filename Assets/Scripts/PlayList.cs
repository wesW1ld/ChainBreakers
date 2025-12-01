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
    private int CardLimit = 2;

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

        return ret;
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
