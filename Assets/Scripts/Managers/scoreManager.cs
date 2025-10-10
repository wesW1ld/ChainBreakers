using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class scoreManager : MonoBehaviour
{
    public static scoreManager instance;

    public int Score { get; private set; }

    public TextMeshProUGUI text;

    public static scoreManager Instance
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
            DontDestroyOnLoad(this);
            Score = 0;
            text.text = $"Score: {Score}";
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

    }

    void OnClick(InputValue value)
    {
        ChangeScore(10000);
    }

    public void ChangeScore(int amt)
    {
        Score += amt;
        text.text = $"Score: {Score}";
    }

    public void ComboEnd()
    {
        //maybe ask the player to pick an enemy to hit
        //call EnemyManager script
        if (Score < 50000) //F
        {
            //call script to deal damage * .5
            //DealDamage(int enemyNum, int dmg);
        }
        else if (Score < 100000) //D
        {
            //damage
        }
        else if (Score < 200000) //C
        {
            //damage * 1.20
        }
        else if (Score < 300000) //B
        {
            //damage * 1.40
        }
        else if (Score < 500000) //A
        {
            //damage * 1.60
        }
        else //S
        {
            //damage * 2
        }
    }

    void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        text.text = $"Score: {Score}";
    }

    public void SetTextObject(TextMeshProUGUI newText)
    {
        text = newText;
        text.text = $"Score: {Score}";
    }
}
