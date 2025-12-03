using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChainBreakers;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();

    private int currentIndex = 0;

    public int maxHandSize;
    public int currentHandSize;
    private HandManager handManager;

    public void Start()
    {
        allCards.Clear();
        Card[] cards = Resources.LoadAll<Card>("Cards/Starting_Cards");
        Debug.Log("Cards loaded: " + cards.Length);
        #if UNITY_EDITOR
            foreach (var c in cards)
            {
                Debug.Log($"Loaded: {c.name} from {AssetDatabase.GetAssetPath(c)}");
            }
        #endif
        allCards.AddRange(cards);
        Debug.Log("allCards loaded: " + allCards.Count);
        handManager = FindObjectOfType<HandManager>();
        if (handManager == null)
        {
            Debug.LogError("DeckManager: HandManager not found in scene!");
            return;
        }
        maxHandSize = handManager.maxHandSize;
        for (int i = 0; i < 5; i++)
        {
            DrawCard();
        }
    }

    void Update()
    {
        if (handManager != null)
        {
            currentHandSize = handManager.cardsInHand.Count;
        }
    } 

    public void DrawCard()
    {
        if (handManager == null)
        {
            Debug.LogError("DeckManager: HandManager is null!");
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
            handManager.DrawCardToHand(nextCard);
            currentIndex = (currentIndex + 1) % allCards.Count;
        }
    }

    public void AddToDeck(Card card)
    {
        allCards.Add(card);
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
