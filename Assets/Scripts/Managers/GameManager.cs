using System.Collections;
using System.Collections.Generic;
using ChainBreakers;
using UnityEditor.XR;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

private int playerHealth;
private int playCardLimit ;
private int handlimit;
private int gold;
private string statusEffect;

public DeckManager deckManager {get ; private set;}
public OptionsManager optionsManager {get ; private set;}
public AudioManger audioManager {get ; private set;}



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        deckManager = GetComponentInChildren<DeckManager>();
        audioManager = GetComponentInChildren<AudioManger>();
        optionsManager = GetComponentInChildren<OptionsManager>();
        if (deckManager == null)
        {
           GameObject prefab = Resources.Load<GameObject>("Prefabs/DeckManager");
           if (prefab == null)
           {
               Debug.Log($"Cannot find DeckManager prefab in Resources/Prefabs/");
           }
           else
            {
                Instantiate(prefab,transform.position,Quaternion.identity,transform);
                deckManager = GetComponentInChildren<DeckManager>();
            }

        }
        if (audioManager== null)
        {
           GameObject prefab = Resources.Load<GameObject>("Prefabs/AudioManager");
           if (prefab == null)
           {
               Debug.Log($"Cannot find AudioManager prefab in Resources/Prefabs/");
           }
           else
            {
                Instantiate(prefab,transform.position,Quaternion.identity,transform);
                audioManager = GetComponentInChildren<AudioManger>();
            }
        }
        if ( optionsManager== null)
        {
           GameObject prefab = Resources.Load<GameObject>("Prefabs/OptionsManager");
           if (prefab == null)
           {
               Debug.Log($"Cannot find OptionsManager prefab in Resources/Prefabs/");
           }
           else
            {
                Instantiate(prefab,transform.position,Quaternion.identity,transform);
                optionsManager = GetComponentInChildren<OptionsManager>();
            }
        }
    }

    public int PlayerHealth
    {
        get { return playerHealth; }
        set { playerHealth = value; }
    }
    public int PlayCardLimit
    {
        get { return playCardLimit; }
        set { playCardLimit = value; }
    }
    public int HandLimit
    {
        get { return handlimit; }
        set { handlimit = value; }
    }
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }
    public string StatusEffect
    {
        get { return statusEffect; }
        set { statusEffect = value; }
    }

}
