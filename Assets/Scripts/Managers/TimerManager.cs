using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    private float totalTime = 0f;
    private bool isRunning = true;

    private void Awake()
    {
        // Singleton setup to ensure only one exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isRunning)
            totalTime += Time.deltaTime;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetTotalTime()
    {
        return totalTime;
    }
}
