using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Target : MonoBehaviour, IPointerClickHandler
{
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        time = 12f;
        Destroy(gameObject, time);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        AimMinigameControl.score += 5000;
        AimMinigameControl.targetsHit += 1;
        AimMinigameControl.check = 1;
        time -= 1f;
        Destroy(gameObject);
    }
}
