using System.Collections;
using System.Collections.Generic;
using ChainBreakers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayArea : MonoBehaviour, IDropHandler
{
    public List<Card> playList = new List<Card>();

    
    private Image playAreaImage;

    void Start()
    {
        playAreaImage = GetComponent<Image>();
    }

    public void AddCard(Card card)
    {
        if (card != null)
        {
            playList.Add(card);
            Debug.Log($"Card added to PlayArea playList: {card.cardName}. Total cards: {playList.Count}");
        }
        else
        {
            Debug.LogWarning("PlayArea.AddCard: received null Card.");
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("PlayArea detected drop event.");
    }
}
