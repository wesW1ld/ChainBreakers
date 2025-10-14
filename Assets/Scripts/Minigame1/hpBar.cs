using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class hpBar : MonoBehaviour
{
    private float HP;
    private float maxHP;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Subscribe());
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateHP()
    {
        HP = playerManager.instance.HP;
        gameObject.GetComponent<UnityEngine.UI.Image>().fillAmount = HP / maxHP; //updates bar image
    }

    IEnumerator Subscribe()
    {
        yield return new WaitForSeconds(.4f);
        playerManager.instance.updateHealth += UpdateHP; //subscribe to updateHealth event, calls UpdateHP() when invoked
        maxHP = playerManager.instance.maxHP;
        UpdateHP();
    }
}
