using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : Enemy
{
    public override void Start()
    {
        base.Start();
        base.currentHP = 300;
        base.maxHP = 300;
        textUIHP.text = $"{currentHP} / {maxHP}";
    }

    public override void MakePMatrix()
    {
        base.NumChoices = 4;
        ProbabiltyMatrix = new float[] { .6f, .0f, .3f, .1f};
        base.MakePMatrix();
    }

    public override void SpecialAttack(int i)
    {
        //weaken player
        PlayerManager.instance.ChangeStatus(PlayerManager.Status.weakend);

        if (i == 3)
        {
            //Debug.Log("Special2");
            if(dazed)
            {
                if(Random.Range(0, 2) == 1)
                {
                    TakeDamage((int)(25f * damageMult));
                    return;
                }
            }
            PlayerManager.instance.TakeDamage((int)(25f * damageMult));
        }
    }

    public override void Attack()
    {
        if(dazed)
        {
            if(Random.Range(0, 2) == 1)
            {
                TakeDamage((int)(50f * damageMult));
                return;
            }
        }
        PlayerManager.instance.TakeDamage((int)(50f * damageMult));
    }

    public override void AttackPreview()
    {
        base.textUI.text = "Attacking for 250";
    }
    
    public override void SpecialAttackPreview(int i)
    {
        if (i == 3)
        {
            base.textUI.text = "Weakening Player and Attacking for 200";
        }
        else
        {
            base.textUI.text = "Weakening Player";
        }
    }

    public override void UpdatePMatrix()
    {
        base.UpdatePMatrix();
        if(base.currentHP * 4 < base.maxHP) //if less than 1/4 of health, do more specials
        {
            if(ProbabiltyMatrix[0] > .3f)
            {
                ProbabiltyMatrix[2] += .2f;
                ProbabiltyMatrix[3] += .1f;
                ProbabiltyMatrix[0] -= .3f;
            }
        }
    }
}
