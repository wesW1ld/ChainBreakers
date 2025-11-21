using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class scoreManager : MonoBehaviour
{
    public static scoreManager instance;

    public int Score; //{ get; private set; }
    public bool minigameMode = false;
    public TextMeshProUGUI text;
    public int baseDmg = 20;

    public GameObject minigame1;
    public GameObject minigame2;

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
        }

    }

    void Start()
    {
        Score = 0;
        text.text = $"Score: {Score}";
        Debug.Log($"[ScoreManager] Initialized with Score = {Score}");
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    public void ChangeScore(int amt)
    {
        Score += amt;
        text.text = $"Score: {Score}";
        Debug.Log($"[ScoreManager] Score changed by {amt}. New Score = {Score}");
    }

    public void ComboEnd()
    {
        EnemyManager.Instance.EnemyAttack();
        //minigame
        // int pick = Random.Range(0, 2);
        // if (pick < 1)
        // {
        //     minigame1.SetActive(true);       //testing
        // }
        // else
        // {
        //     minigame2.SetActive(true);
        // }        
        // minigameMode = true;
    }

    public void MinigameEnd(int num)
    {
        //damage
        if (Score < 50000) //F
        {
            EnemyManager.Instance.DealDamage((int)(baseDmg * .5));
        }
        else if (Score < 100000) //D
        {
            EnemyManager.Instance.DealDamage(baseDmg);
        }
        else if (Score < 200000) //C
        {
            EnemyManager.Instance.DealDamage((int)(baseDmg * 2));
        }
        else if (Score < 300000) //B
        {
            EnemyManager.Instance.DealDamage((int)(baseDmg * 3));
        }
        else if (Score < 500000) //A
        {
            EnemyManager.Instance.DealDamage((int)(baseDmg * 5));
        }
        else //S
        {
            EnemyManager.Instance.DealDamage((int)(baseDmg * 5));
            EnemyManager.Instance.DealDamage((int)(baseDmg * 5));
        }

        minigameMode = false;
        if(num == 1)
        {
            minigame1.SetActive(false);
        }
        else if(num == 2)
        {
            minigame2.SetActive(false);
        }
    }

    void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        text.text = $"Score: {Score}";
        Debug.Log($"[ScoreManager] Scene changed to {newScene.name}. Current Score = {Score}");
    }

    public void SetTextObject(TextMeshProUGUI newText)
    {
        text = newText;
        text.text = $"Score: {Score}";
        Debug.Log($"[ScoreManager] Text object set. Current Score = {Score}");
    }
}
