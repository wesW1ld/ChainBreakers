using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //singleton stuff
    public static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameManager is Null");
            }
            return instance;
        }

    }
    private void Awake()
    {

        if (instance)
        {
            Debug.LogError("GameManager is already in the scene");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            Debug.Log("playermanager awakened");
            DontDestroyOnLoad(this); //stay over scene
        }

    }

    

    private int HP = 1000;
    private int maxHP = 1000;
    private int Shield = 0;

    public event System.Action UpdateHealth;
    public event System.Action UpdateShield;

    void Start()
    {
        UpdateHealth?.Invoke();
        UpdateShield?.Invoke();
    }
    
    public void TakeDamage(int amt) //called from Telegraph.cs
    {
        amt -= Shield;
        if(amt < 0){amt = 0;}
        HP -= amt;
        if(HP < 0){HP = 0;}
        UpdateHealth?.Invoke(); //invokes updateHealth event, which tells the hpBar.cs to update the healthbar
    }

    public int GetHp()
    {
        return HP;
    }

    public void Heal(int amt)
    {
        HP += amt;
        if(HP > maxHP){HP = maxHP;}
        UpdateHealth?.Invoke();
    }

    public int GetShield()
    {
        return Shield;
    }

    public void AddShield(int amt)
    {
        Shield += amt;
        UpdateShield?.Invoke();
    }

    public void RemoveShield()
    {
        Shield = 0;
        UpdateShield?.Invoke();
    }

    public enum Status
    {
        normal,
        poisoned,
        weakend
    }

    private Status curStatus = Status.normal;

    public void ChangeStatus(Status s)
    {
        curStatus = s;
        Debug.Log($"Player status changed to {curStatus}");
    }
}
