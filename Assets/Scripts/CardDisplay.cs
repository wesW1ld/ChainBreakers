using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChainBreakers;

[System.Serializable]
public class CardTypeImage
{
    public Card.CardType cardType;
    public Sprite backgroundSprite;
}

public class CardDisplay : MonoBehaviour
{

    public Card card;
    public Image cardImage;
    public Image cardBackgroundImage;
    public TMP_Text cardName;
    public TMP_Text cardDescription;
    public TMP_Text min;
    public TMP_Text max;
    public TMP_Text minTurn;
    public TMP_Text maxTurn;

    [SerializeField] private List<CardTypeImage> cardTypeImages = new List<CardTypeImage>();


    public void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        if (card == null)
        {
            Debug.LogWarning("CardDisplay: 'card' is not assigned on " + gameObject.name + ". Assign a Card ScriptableObject in the inspector.");
            return;
        }

        // Update card background image based on card type
        if (cardBackgroundImage != null)
        {
            CardTypeImage typeImage = cardTypeImages.Find(x => x.cardType == card.cardType);
            if (typeImage != null && typeImage.backgroundSprite != null)
                cardBackgroundImage.sprite = typeImage.backgroundSprite;
            else
                Debug.LogWarning($"CardDisplay: No background sprite found for card type '{card.cardType}' on {gameObject.name}.");
        }

        // Update UI fields only if they are assigned to avoid NullReferenceExceptions
        if (cardName != null)
            cardName.text = card.cardName ?? "";
        else
            Debug.LogWarning($"CardDisplay: 'cardName' TMP_Text is not assigned on {gameObject.name}.");

        if (cardDescription != null)
            cardDescription.text = card.description ?? "";
        else
            Debug.LogWarning($"CardDisplay: 'cardDescription' TMP_Text is not assigned on {gameObject.name}.");

        if (cardImage != null)
            cardImage.sprite = card.artwork;
        else
            Debug.LogWarning($"CardDisplay: 'cardImage' Image is not assigned on {gameObject.name}.");

        if (min != null)
            min.text = card.min.ToString();
        else
            Debug.LogWarning($"CardDisplay: 'min' TMP_Text is not assigned on {gameObject.name}.");

        if (max != null)
            max.text = card.max.ToString();
        else
            Debug.LogWarning($"CardDisplay: 'max' TMP_Text is not assigned on {gameObject.name}.");

        if (minTurn != null)
            minTurn.text = card.minTurn.ToString();
        else
            Debug.LogWarning($"CardDisplay: 'minTurn' TMP_Text is not assigned on {gameObject.name}.");

        if (maxTurn != null)
            maxTurn.text = card.maxTurn.ToString();
        else
            Debug.LogWarning($"CardDisplay: 'maxTurn' TMP_Text is not assigned on {gameObject.name}.");
    }
}
