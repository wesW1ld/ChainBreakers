using System.Collections;
using System.Collections.Generic;
using ChainBreakers;
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
    
}
