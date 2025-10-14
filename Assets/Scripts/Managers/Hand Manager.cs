using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab; //assign cardprefab inspector
    public Transform handTransform;//root of the hand position
    public float fanSpread = 5f;
    public List<GameObject> cardsInHand = new List<GameObject>(); //holds list of card objects in hand


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 7; i++)
             AddCardToHand();

    }

    private void AddCardToHand()
    {
        //instantiate card
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);

        UpdateHandVisuals();
    }

    private void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;
        for(int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount-1))/2f);
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

        }
    }
}
