using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour
{
    private Transform dragging = null;
    private Vector3 offset;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                dragging = hit.transform;
                offset = dragging.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragging = null;
        }

        if (dragging != null)
        {
            dragging.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
    }

    private IEnumerator addDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
