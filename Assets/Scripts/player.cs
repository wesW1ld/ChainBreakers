using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class player : MonoBehaviour
{
    public int attackDamage = 10;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            scoreManager.instance.ComboEnd();
        }
    }
}
