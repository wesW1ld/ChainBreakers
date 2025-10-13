using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChainBreakers;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public TMP_Text cardName;
    public TMP_Text cardDescription;

    void Start ()
    {
        if (cardData != null)
        {
            cardName.text = cardData.cardName;
            cardDescription.text = cardData.description;
            cardImage.sprite = cardData.artwork;
        }
        else
        {
            Debug.LogWarning("Card data is not assigned.");
        }
    }
}
