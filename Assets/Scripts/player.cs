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

    void OnQ()
    {
        PlayList.instance.Push(attack);
    }
    void OnW()
    {
        PlayList.instance.Push(defense);
    }
    void OnE()
    {
        PlayList.instance.Push(status);
    }
    void OnR()
    {
        PlayList.instance.Push(special);
    }
    void OnGo()
    {
        scoreManager.instance.ComboEnd();
    }
}
