using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChainBreakers;

public class CardDisplay : MonoBehaviour
{

    public Card card;
    public Image cardImage;
    public TMP_Text cardName;
    public TMP_Text cardDescription;
    public TMP_Text min;
    public TMP_Text max;

    public void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
    if (card == null) return;

    cardName.text = card.cardName;
    cardDescription.text = card.description;
    cardImage.sprite = card.artwork;
    min.text = card.minDam.ToString();
    max.text = card.maxDam.ToString();

    }
}
