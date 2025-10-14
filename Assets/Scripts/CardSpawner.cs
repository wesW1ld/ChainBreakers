using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChainBreakers;
using Unity.VisualScripting;
using System;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform handArea;
    public float fanSpread = 5f;
    public List<Card> hand = new List<Card>();
    public List<GameObject> handObejects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            //Card randomcard = DeckManager.Card(DeckManager.CardName.ShortStrike,DeckManager.CardType.Atk);
            //hand.Add(randomcard);
        }
        SpawnCards();
    }

    // Update is called once per frame
    void SpawnCards()
    {
        foreach (GameObject obj in handObejects)
        {
            Destroy(obj);
        }

        handObejects.Clear();

        foreach (Card data in hand)
        {
            GameObject newCard = Instantiate(cardPrefab, handArea);
            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            cardDisplay.Setupcard(data);

            handObejects.Add(newCard);
        }
        UpdateHandVisuals();
    }

    private void UpdateHandVisuals()
    {
        int cardCount = hand.Count;
        float startAngle = -fanSpread * (cardCount - 1) / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            float angle = startAngle + i * fanSpread;
            Transform cardTransform = handObejects[i].transform;

            cardTransform.localRotation = Quaternion.Euler(0, 0, angle);
            cardTransform.localPosition = new Vector3(i * 2f - (cardCount - 1) * 1f, 0, 0);
        }
    }
}
