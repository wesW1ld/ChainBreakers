using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class GrizzlyBear : Enemy
{
    public override void MakePMatrix()
    {
        ProbabiltyMatrix = new float[] {.4f, .0f,.6f};
        base.MakePMatrix();
    }

    public override void SpecialAttack(int i)
    {
        //bleed the player
        PlayerManager.instance.ChangeStatus(PlayerManager.Status.bleeding);
    }

    public override void SpecialAttackPreview(int i)
    {
        base.textUI.text = "You are bleeding!";
    }

}
