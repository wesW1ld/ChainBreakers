using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChainBreakers;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();

    private int currentIndex = 0;

    public void Start()
    {
        Card[] cards = Resources.LoadAll<Card>("Cards");
        allCards.AddRange(cards);
        for (int i = 0; i < 5; i++)
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        if (allCards.Count == 0)
        {
            Debug.Log("No more cards in the deck!");
            return;
        }

        Card nextCard = allCards[currentIndex];
        HandManager.instance.DrawCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % allCards.Count;
    }

    public static DeckManager instance;
    public static DeckManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("DeckManager is Null");
            }
            return instance;
        }

    }
    private void Awake()
    {

        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this); //stay over scene
        }

    }
}
