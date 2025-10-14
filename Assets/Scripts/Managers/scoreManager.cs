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

    public TextMeshProUGUI text;

    public int baseDmg = 20;

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

    void OnClick(InputValue value)
    {
        ChangeScore(10000);
    }

    public void ChangeScore(int amt)
    {
        Score += amt;
        text.text = $"Score: {Score}";
        Debug.Log($"[ScoreManager] Score changed by {amt}. New Score = {Score}");
    }

    public void ComboEnd()
    {
        //maybe ask the player to pick an enemy to hit
        //call EnemyManager script
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
            EnemyManager.Instance.DealDamage((int)(baseDmg * 4));
        }
        else //S
        {
            EnemyManager.Instance.DealDamage((int)(baseDmg * 5));
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
