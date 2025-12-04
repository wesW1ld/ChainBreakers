using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Giant : Enemy
{
    public override void MakePMatrix()
    {
        ProbabiltyMatrix = new float[] {.9f, .0f,.1f};
        base.MakePMatrix();
    }

    public override void SpecialAttack(int i)
    {
        //bleed the player
        PlayerManager.instance.ChangeStatus(PlayerManager.Status.weakend);
    }

    public override void SpecialAttackPreview(int i)
    {
        base.textUI.text = "You have been weakend!";
    }

}
