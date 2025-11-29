using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpText : MonoBehaviour
{
    TextMeshProUGUI text;

    int Hp;
    int Shield;

    void Start()
    {
        PlayerManager.instance.UpdateHealth += UpdateHp;
        PlayerManager.instance.UpdateShield += UpdateShield;
        text = GetComponent<TextMeshProUGUI>();
    }

    private void UpdateHp()
    {
        Hp = PlayerManager.instance.GetHp();
        text.text = $"HP: {Hp}\nShield: {Shield}";
    }

    private void UpdateShield()
    {
        Shield = PlayerManager.instance.GetShield();
        text.text = $"HP: {Hp}\nShield: {Shield}";
    }
}
