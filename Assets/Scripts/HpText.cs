using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpText : MonoBehaviour
{
    TextMeshProUGUI text;

    float Hp;

    void Start()
    {
        PlayerManager.instance.UpdateHealth += UpdateHp;
        text = GetComponent<TextMeshProUGUI>();
    }

    private void UpdateHp()
    {
        Hp = PlayerManager.instance.GetHp();
        text.text = $"HP: {Hp}";
    }
}
