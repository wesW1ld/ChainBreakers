using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneTransitioner : MonoBehaviour
{
    public static SceneTransitioner instance;

    public static SceneTransitioner Instance
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
        StartCoroutine(Next());
    }
    void OnClick(InputValue value)
    {
        SceneManager.LoadScene(3);
    }

    IEnumerator Next()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(3);
    }
}
