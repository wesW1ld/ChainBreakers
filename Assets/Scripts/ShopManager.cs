using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChainBreakers;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ShopManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    private int currentIndex = 0;

    public int maxHandSize;
    public int currentHandSize;

    private testManager headManager;

    public static ShopManager instance;
    public static ShopManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("ShopManager is Null");
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
            DontDestroyOnLoad(this);
        }
    }

    public void Start()
    {
        allCards.Clear();

        Card[] cards = Resources.LoadAll<Card>("Cards/Area_1/Area_1_Shop_Cards");
        allCards.AddRange(cards);

        headManager = FindObjectOfType<testManager>();
        if (headManager == null)
        {
            Debug.LogError("ShopManager: testManager not found in scene!");
            return;
        }

        maxHandSize = headManager.maxHandSize;

        for (int i = 0; i < 3; i++)
        {
            DrawCard();
        }
    }

    void Update()
    {
        if (headManager != null)
        {
            currentHandSize = headManager.cardsInHand.Count;
        }
    }

    public void DrawCard()
    {
        if (headManager == null)
        {
            Debug.LogError("ShopManager: testManager is null!");
            return;
        }

        if (allCards.Count == 0)
        {
            Debug.Log("No more cards in the deck!");
            return;
        }

        if (currentHandSize < maxHandSize)
        {
            Card nextCard = allCards[currentIndex];
            headManager.DrawCardToHand(nextCard);
            currentIndex = (currentIndex + 1) % allCards.Count;
        }
    }
}
