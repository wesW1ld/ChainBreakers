using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChainBreakers;

public class CardDisplay_Later : MonoBehaviour
{
    public Image cardImage;
    public TMP_Text cardName;
    public TMP_Text cardDescription;

    private DeckManager.Card cardData;

    public void SetupCard (DeckManager.Card Card)
    {
        cardData = Card;
        cardName.text = Card.name.ToString();
        cardDescription.text = Card.description;
        cardImage.sprite = Card.artwork;
    }
}
