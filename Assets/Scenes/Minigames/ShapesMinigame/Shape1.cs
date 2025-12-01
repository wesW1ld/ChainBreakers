using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape1 : MonoBehaviour, IPointerClickHandler
{
    public static int correct;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (correct == 1)
        {
            ShapesMinigameControl.score += 5000;
            ShapesMinigameControl.shapesChained += 1;
            ShapesMinigameControl.check = 1;
        }
        else
        {
            ShapesMinigameControl.check = 2;
        }
        Destroy(gameObject);
    }
}
