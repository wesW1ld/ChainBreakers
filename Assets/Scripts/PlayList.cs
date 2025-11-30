using System.Collections;
using System.Collections.Generic;
using ChainBreakers;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayList : MonoBehaviour
{
    private Stack<Card> PlayedCards = new Stack<Card>();
    //private Stack<GameObject> CardObjects = new Stack<GameObject>();
    private int CardLimit = 100;

    public int enemyNum = 0;

    public void Push(Card card, GameObject cardO) //hand is gonna move cards, then when turn is over, push them all in order
    {
        if(PlayedCards.Count == CardLimit)
        {
            Debug.Log("Max number of cards played");
            return;
        }
        PlayedCards.Push(card);
        //CardObjects.Push(cardO);
    }

    public void Pop()
    {
        PlayedCards.Pop();
        //CardObjects.Pop();
    }

    public Card[] SeeStack() //see current stack with top being index 0
    {
        Card[] ret = PlayedCards.ToArray();
        return ret;
    }

    public Card Top()
    {
        return PlayedCards.Peek();
    }

    public void Clear()
    {
        foreach(Card c in PlayedCards)
        {
            Pop();
        }
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

        for (int i = items.Length - 1; i >= 0; i--)
        {
            Card card = items[i];

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
                EnemyManager.Instance.DealDamage((int)(mult * Random.Range(card.min, card.max + 1)), enemyNum);
            }
            else if(card.cardType == Card.CardType.Defend)
            {
                PlayerManager.instance.AddShield(Random.Range(card.min, card.max + 1));
            }

            //depends on the card, not including status effects done below
            if(card.cardName == "Medical Training")
            {
                PlayerManager.instance.Heal(Random.Range(card.min, card.max + 1));
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
        }
    } 
}
