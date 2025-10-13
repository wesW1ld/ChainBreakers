using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChainBreakers;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform handArea;
    public List<Card> hand;

    // Start is called before the first frame update
    void Start()
    {
        SpawnCards();
    }

    // Update is called once per frame
    void SpawnCards()
    {
        foreach(Card data in hand)
        {
            GameObject newCard = Instantiate(cardPrefab, handArea);
            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            hand.Add(data);
        }
    }
}
