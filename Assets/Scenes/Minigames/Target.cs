using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
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
    
    private void OnMouseDown()
    {
        AimMinigameControl.score += 5;
        AimMinigameControl.targetsHit += 1;
        AimMinigameControl.check = 1;
        time -= 1f;
        Destroy(gameObject);
    }
}
