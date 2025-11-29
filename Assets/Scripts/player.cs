using System.Collections;
using System.Collections.Generic;
using ChainBreakers;
using Unity.Mathematics;
using UnityEngine;

public class player : MonoBehaviour
{
    public int attackDamage = 10;

    public Card attack;
    public Card defense;
    public Card status;
    public Card special;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            scoreManager.instance.ComboEnd();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayList.instance.Push(attack, null);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayList.instance.Push(defense, null);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayList.instance.Push(status, null);
        }
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            PlayList.instance.Push(special, null);
        }
    }
}
