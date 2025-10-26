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
    public TMP_Text minTurn;
    public TMP_Text maxTurn;

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
    if (card.min != 0)min.text = card.min.ToString();
    if (card.max != 0)max.text = card.max.ToString();
    if (card.minTurn != 0)minTurn.text = card.minTurn.ToString();
    if (card.maxTurn != 0)maxTurn.text = card.maxTurn.ToString();
    }
}
