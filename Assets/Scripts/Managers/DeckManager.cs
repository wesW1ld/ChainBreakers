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
    }

    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0)
        {
            Debug.Log("No more cards in the deck!");
            return;
        }

        Card nextCard = allCards[currentIndex];
        handManager.DrawCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % allCards.Count;
    }
}
