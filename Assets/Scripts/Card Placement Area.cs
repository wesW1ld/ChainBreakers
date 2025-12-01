using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardPlacementArea : MonoBehaviour
{
    public List<GameObject> placedCards = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider card)
    {

    }
}
