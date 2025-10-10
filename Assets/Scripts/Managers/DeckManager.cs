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
        HunkerDown,
        RageBait,
        ScoutScan
    }

    public struct Card
    {
        public CardName name;
        public CardType type;
        public int lower;
        public int higher;

        public Card(CardName name, CardType type)
        {
            this.name = name;
            this.type = type;
            this.lower = 0;
            this.higher = 0;
        }
        public Card(CardName name, CardType type, int lower, int upper)
        {
            this.name = name;
            this.type = type;
            this.lower = lower;
            this.higher = upper;
        }
    }

    public void UseCard(Card card)
    {
        switch (card.name)
        {
            case CardName.ShortStrike:
                //based on score do card.lower to card.higher damage
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
        deck.Add(new Card(CardName.ShortStrike, CardType.Atk, 5, 10));
    }
}
