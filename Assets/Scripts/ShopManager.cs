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

    public void Start()
    {
        allCards.Clear();
        Card[] cards = Resources.LoadAll<Card>("Cards/Area_1/Area_1_Shop_Cards");
        Debug.Log("Cards loaded: " + cards.Length);
#if UNITY_EDITOR
            foreach (var c in cards)
            {
                Debug.Log($"Loaded: {c.name} from {AssetDatabase.GetAssetPath(c)}");
            }
#endif
        allCards.AddRange(cards);
        Debug.Log("allCards loaded: " + allCards.Count);
        testManager = FindObjectOfType<testManager>();
        if (testManager == null)
        {
            Debug.LogError("DeckManager: testManager not found in scene!");
            return;
        }
        maxHandSize = testManager.maxHandSize;
        for (int i = 0; i < 3; i++)
        {
            DrawCard();
        }
    }

    void Update()
    {
        if (testManager != null)
        {
            currentHandSize = testManager.cardsInHand.Count;
        }
    }

    public void DrawCard()
    {
        if (testManager == null)
        {
            Debug.LogError("DeckManager: testManager is null!");
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
            testManager.DrawCardToHand(nextCard);
            currentIndex = (currentIndex + 1) % allCards.Count;
        }
    }

    public void AddToDeck(Card card)
    {
        allCards.Add(card);
    }

    public static ShopManager instance;
    public static ShopManager Instance
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
            DontDestroyOnLoad(this); //stay over scene
        }

    }
}
