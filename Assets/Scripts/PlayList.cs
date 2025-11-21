using System.Collections;
using System.Collections.Generic;
using ChainBreakers;
using UnityEngine;

public class PlayList : MonoBehaviour
{
    Stack<Sprite> CardPics = new Stack<Sprite>();
    Stack<Card> PlayedCards = new Stack<Card>();
    private int CardLimit = 100;

    public void Push(Card card)//took away sprite for testing
    {
        if(PlayedCards.Count == CardLimit)
        {
            Debug.Log("Max number of cards played");
            return;
        }
        //CardPics.Push(sprite);
        PlayedCards.Push(card);
    }

    public void Pop()
    {
        CardPics.Pop();
        PlayedCards.Pop();
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

    private void UpdateDisplay()
    {
        
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
