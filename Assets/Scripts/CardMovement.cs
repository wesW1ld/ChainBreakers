using UnityEngine;
using UnityEngine.EventSystems;
using ChainBreakers;
using System.Collections.Generic;


public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private Vector3 orginalScale;
    private int currentState = 0;
    private Quaternion originalRotation;
    private Vector3 orginalPosition;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;
    [SerializeField] private float lerpFactor = 0.1f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        orginalScale = rectTransform.localScale;
        originalRotation = rectTransform.rotation;
        orginalPosition = rectTransform.localPosition;
    }

    void Update()
    {
        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;
            case 2:
                HandleDragState();
                if (!Input.GetMouseButton(0))
                {
                    TransitionToState0();
                }
                break;
            case 3:
                HandlePlayState();
                if (!Input.GetMouseButton(0))
                {
                    // Check if card was dropped on PlayArea
                    CheckPlayAreaDrop();
                    TransitionToState0();
                }
                break;

        }
    }

    private void TransitionToState0()
    {
        currentState = 0;
        rectTransform.localScale = orginalScale;
        rectTransform.localRotation = originalRotation;
        rectTransform.localPosition = orginalPosition;
        glowEffect.SetActive(false);
        playArrow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            orginalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            orginalScale = rectTransform.localScale;

            currentState = 1;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            TransitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            currentState = 2;
            // Store the offset between the pointer position and the card's current position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(), 
                eventData.position, 
                eventData.pressEventCamera, 
                out originalLocalPointerPosition);
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == 2 || currentState == 3)
        {
            // Convert screen position to canvas-local position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(), 
                eventData.position, 
                eventData.pressEventCamera, 
                out Vector2 localPointerPosition);
            
            // Calculate offset from where you initially clicked
            Vector2 offset = localPointerPosition - originalLocalPointerPosition;
            
            // Set card position to follow cursor with the calculated offset
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, orginalPosition + (Vector3)offset, lerpFactor);

            // Check if card is above the play threshold to enter play state
            if (currentState == 2 && rectTransform.localPosition.y > cardPlay.y)
            {
                currentState = 3;
                playArrow.SetActive(true);
            }
            // Check if card drops below threshold to exit play state
            else if (currentState == 3 && rectTransform.localPosition.y < cardPlay.y)
            {
                currentState = 2;
                playArrow.SetActive(false);
            }
        }
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = orginalScale * selectScale;

    }

    private void HandleDragState()
    {
        rectTransform.localRotation = Quaternion.identity;
    }
    
    private void HandlePlayState()
    {
        rectTransform.localRotation = Quaternion.identity;
    }

    private void CheckPlayAreaDrop()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            PlayArea playArea = result.gameObject.GetComponent<PlayArea>();
            if (playArea != null)
            {
                CardDisplay cardDisplay = GetComponent<CardDisplay>();
                if (cardDisplay != null && cardDisplay.card != null)
                {
                    playArea.AddCard(cardDisplay.card);
                    Debug.Log($"Card '{cardDisplay.card.cardName}' dropped on PlayArea and removed.");
                    Destroy(gameObject);
                    return;
                }
                break;
            }
        }
    }
}
