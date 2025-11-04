using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
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
        base.textUI.text = "Weakening Player";

        if (i == 3)
        {
            Debug.Log("Special2");
            PlayerManager.instance.TakeDamage(200f);
            base.textUI.text = "Weakening Player and Attacking for 200";
        }
    }

    public override void Attack()
    {
        PlayerManager.instance.TakeDamage(250f);
        base.textUI.text = "Attacking for 250";
    }
}
