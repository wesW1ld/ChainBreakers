using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManagerM1 : MonoBehaviour
{
    public static playerManagerM1 instance;

    public float HP { get; private set; }
    public float maxHP { get; private set; }

    public event System.Action updateHealth;

    //makes the script a singleton, call methods and get variables with playerManagerM1.instance
    public static playerManagerM1 Instance
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
            Debug.Log("playermanagerM1 awakened");
            //DontDestroyOnLoad(this); 
        }

    }
    // Start is called before the first frame update
    void OnEnable()
    {
        maxHP = 3;
        HP = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float amt) //called from Telegraph.cs
    {
        HP -= amt;
        updateHealth?.Invoke(); //invokes updateHealth event, which tells the hpBar.cs to update the healthbar
    }
}
