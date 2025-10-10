using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    void Start()
    {
        scoreManager.instance.SetTextObject(GetComponent<TextMeshProUGUI>());
    }
}
