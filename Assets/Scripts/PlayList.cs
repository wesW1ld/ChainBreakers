using System.Collections;
using System.Collections.Generic;
using ChainBreakers;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayList : MonoBehaviour
{
    private Stack<Card> PlayedCards = new Stack<Card>();
    private Stack<GameObject> CardObjects = new Stack<GameObject>();
    private int CardLimit = 100;

    public void Push(Card card, GameObject cardO) //hand is gonna move cards, then when turn is over, push them all in order
    {
        if(PlayedCards.Count == CardLimit)
        {
            Debug.Log("Max number of cards played");
            return;
        }
        PlayedCards.Push(card);
        CardObjects.Push(cardO);
    }

    public void Pop()
    {
        PlayedCards.Pop();
        CardObjects.Pop();
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
                EnemyManager.Instance.DealDamage(UnityEngine.Random.Range(card.min, card.max), 0);
            }
            else if(card.cardType == Card.CardType.Defend)
            {
                //add min to max sheild
            }
            else
            {
                //depends on the card, not including status effects done below
            }

            foreach(Card.StatusEffect sta in card.statusEffects)
            {
                //apply to enemy
            }
        }
    } 
}
