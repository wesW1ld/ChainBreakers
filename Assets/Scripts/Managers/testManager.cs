///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChainBreakers;
using Unity.VisualScripting;

public class testManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform handTransform;
    public float fanSpread = 9.15f;
    public List<GameObject> cardsInHand = new List<GameObject>();
    public float cardSpacing = 230f;
    public int maxHandSize = 7;
    private int curHandSize = 0;

    //public void DrawCardToHand(Card cardData)
    //{
    //    if(curHandSize >= maxHandSize){return;}
    //    GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
    //    cardsInHand.Add(newCard);

    //    newCard.GetComponent<CardDisplay>().card = cardData;

    //    //UpdateHandLayout();
    //}
    public void DrawCardToHand(Card cardData)
    {
        if (curHandSize >= maxHandSize) return;

        // Base starting position
        Vector3 startPos = new Vector3(-20f, 0f, 0f);

        // Each card is 500 units to the right
        float xOffset = cardsInHand.Count * 500f;
        Vector3 newPos = startPos + new Vector3(xOffset, 0f, 0f);

        // Instantiate card
        GameObject newCard = Instantiate(cardPrefab, handTransform);
        newCard.transform.localPosition = newPos;

        // Set card data
        newCard.GetComponent<CardDisplay>().card = cardData;

        // Track card
        cardsInHand.Add(newCard);
        curHandSize++;
    }



    public void RemoveCard(GameObject card)
    {
        curHandSize--;
        cardsInHand.Remove(card);
        //UpdateHandLayout();
    }

    public void AddCard(GameObject card)
    {
        curHandSize++;
        cardsInHand.Add(card);
        //UpdateHandLayout();
    }

    public void UpdateHandLayout()
    {
        if (cardsInHand.Count == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        int cardCount = cardsInHand.Count;
        float radius = cardSpacing * cardCount * 0.5f;
        float totalAngle = fanSpread * (cardCount - 1);
        float startAngle = -totalAngle * 0.5f;

        for (int i = 0; i < cardCount; i++)
        {
            // Calculate the angle for this card
            float angle = startAngle + (fanSpread * i);
            float radians = angle * Mathf.Deg2Rad;

            // Calculate position on the arc (inverted)
            float xPos = Mathf.Sin(radians) * radius;
            float yPos = Mathf.Cos(radians) * radius * 0.3f; // Removed negative sign to invert

            // Set position and rotation (negative angle for inverted fan)
            cardsInHand[i].transform.localPosition = new Vector3(xPos, yPos, 0f);
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, -angle); // Negative angle
        }
    }

    public int HandSizeDif()
    {
        return maxHandSize - curHandSize;
    }

    public static testManager instance;
    public static testManager Instance
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
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this); //stay over scene
        }

    }
}