using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    public static playerManager instance;

    public float HP { get; private set; }
    public float maxHP { get; private set; }

    public event System.Action updateHealth;

    //makes the script a singleton, call methods and get variables with playerManager.instance
    public static playerManager Instance
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
