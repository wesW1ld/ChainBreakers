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

    

    private float HP = 1000f;

    public event System.Action UpdateHealth;

    void Start()
    {
        UpdateHealth?.Invoke();
    }
    
    public void TakeDamage(float amt) //called from Telegraph.cs
    {
        HP -= amt;
        UpdateHealth?.Invoke(); //invokes updateHealth event, which tells the hpBar.cs to update the healthbar
    }

    public float GetHp()
    {
        return HP;
    }

    public enum Status
    {
        normal,
        poisoned,
        fire,
        weakend
    }

    private Status curStatus = Status.normal;

    public void ChangeStatus(Status s)
    {
        curStatus = s;
        Debug.Log($"Player status changed to {curStatus}");
    }
}
