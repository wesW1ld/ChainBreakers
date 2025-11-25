using System.Collections;
using System.Collections.Generic;
using ChainBreakers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    public int attackDamage = 10;

    public Card attack;
    public Card defense;
    public Card status;
    public Card special;

    void OnQkey(InputValue value)
    {
        Debug.Log("Pressed");
        PlayList.instance.Push(attack, null);
    }
    void OnWkey(InputValue value)
    {
        Debug.Log("Pressed");
        PlayList.instance.Push(defense, null);
    }
    void OnEkey(InputValue value)
    {
        Debug.Log("Pressed");
        PlayList.instance.Push(status, null);
    }
    void OnRkey(InputValue value)
    {
        Debug.Log("Pressed");
        PlayList.instance.Push(special, null);
    }
    void OnGo(InputValue value)
    {
        Debug.Log("Pressed");
        scoreManager.instance.ComboEnd();
    }

    void OnMove(InputValue value)
    {
        Debug.Log("movement");
    }
}
