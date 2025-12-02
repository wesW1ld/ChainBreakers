using UnityEngine;
using UnityEngine.EventSystems;
using ChainBreakers;
using System.Collections.Generic;


public class CardMovement : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Play Stuff
    bool inPlay = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        if(!inPlay) //put into play
        {
            if(!PlayList.instance.IsFull())
            {
                MoveCardToLeftSide();//move to area
                PlayList.instance.Push(gameObject.GetComponent<CardDisplay>().card, gameObject);
                HandManager.instance.RemoveCard(gameObject);
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                inPlay = !inPlay; //don't put into play
            }
            
        }
        else //remove from play
        {
            PlayList.instance.Pop(gameObject.GetComponent<CardDisplay>().card, gameObject);
            HandManager.instance.AddCard(gameObject);
        }
        inPlay = !inPlay;
    }

    private float displacementMult = 20;

    private void MoveCardToLeftSide()
    {
        transform.position = PlayList.instance.transform.position + new Vector3(PlayList.instance.GetSize() * displacementMult, 0, 0);
    }



    //Hover Stuff
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private float selectScale = 1.1f;
    private RectTransform rectTransform;
    private Vector3 orginalScale;
    private int originalIndex;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        orginalScale = rectTransform.localScale;
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = orginalScale * selectScale;
        originalIndex = rectTransform.GetSiblingIndex();
        rectTransform.SetAsLastSibling();
    }

    private void LeaveHover()
    {
        glowEffect.SetActive(false);
        rectTransform.localScale = orginalScale;
        rectTransform.SetSiblingIndex(originalIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HandleHoverState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeaveHover();
    }
}