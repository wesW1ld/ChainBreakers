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
            PlayerManager.instance.TakeDamage((int)(200f * damageMult));
        }
    }

    public override void Attack()
    {
        PlayerManager.instance.TakeDamage((int)(250f * damageMult));
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
