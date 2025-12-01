using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChainBreakers;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameObject cardPrefab;
    public Transform handTransform;
    public float fanSpread = 9.15f;
    public List<GameObject> cardsInHand = new List<GameObject>();
    public float cardSpacing = 230f;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void DrawCardToHand(Card cardData)
    {
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);

        newCard.GetComponent<CardDisplay>().card = cardData;

        UpdateHandLayout();
    }

    public void RemoveCard(GameObject card)
    {
        cardsInHand.Remove(card);
        UpdateHandLayout();
    }

    public void AddCard(GameObject card)
    {
        cardsInHand.Add(card);
        UpdateHandLayout();
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

    public static HandManager instance;
    public static HandManager Instance
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
