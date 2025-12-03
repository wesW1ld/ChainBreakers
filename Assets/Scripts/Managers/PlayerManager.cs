using System.Collections;
using System.Collections.Generic;
using ChainBreakers;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            //DontDestroyOnLoad(this); //stay over scene
        }

    }

    
    private int HP = 250;
    private int maxHP = 250;

    private int Shield = 0;
    private bool might = false;
    private bool poise = false;
    private bool regenerative = false;
    private int mTimer = 0;
    private int pTimer = 0;
    private int rTimer = 0;

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
        if(poise)
        {
            amt = (int)(amt / 1.5f);
        }
        if(amt < 0){amt = 0;}
        HP -= amt;
        if(HP < 0){HP = 0;}
        DeathCheck();
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

    //update player buffs 0:might 1:poise
    public void Buff(Card.StatusEffect type, int length)
    {
        if(type == Card.StatusEffect.might)
        {
            might = true;
            mTimer = length;
        }
        else if(type == Card.StatusEffect.poise)
        {
            poise = true;
            pTimer = length;
        }
        else
        {
            regenerative = true;
            rTimer = length;
        }
    }

    //call every cycle to update buff timers
    public void Timer()
    {
        //health stuff
        if(regenerative)
        {
            Heal((int)(HP*.2f)); //heal 20%
        }
        if(curStatus == Status.poisoned)
        {
            HP -= (int)(maxHP * .2f);//take 20% of max
            if(HP < 0){HP = 0;}
            DeathCheck();
            UpdateHealth?.Invoke();
        }

        //buff stuff
        mTimer--;
        pTimer--;
        rTimer--;
        if(mTimer == 0)
        {
            might = false;
        }
        else if(mTimer < 0){mTimer = 0;}
        if(pTimer == 0)
        {
            poise = false;
        }
        else if(pTimer < 0){pTimer = 0;}
        if(rTimer == 0)
        {
            regenerative = false;
        }
        else if(rTimer < 0){rTimer = 0;}

        //negative status stuff
        statusTimer--;
        if(statusTimer == 0)
        {
            curStatus = Status.normal;
        }
        else if(statusTimer < 0){statusTimer = 0;}
    }

    public bool MightCheck()
    {
        return might;
    }

    public enum Status
    {
        normal,
        poisoned,
        weakend
    }

    private Status curStatus = Status.normal;
    private int statusTimer = 0;

    public void ChangeStatus(Status s)
    {
        curStatus = s;
        statusTimer = 3;
        Debug.Log($"Player status changed to {curStatus}");
    }

    public bool WeakendCheck()
    {
        return curStatus == Status.weakend;
    }

    private void DeathCheck()
    {
        if(HP <= 0)
        {
            Debug.Log("player died");
            SceneManager.LoadScene(3);
        }
    }
}
