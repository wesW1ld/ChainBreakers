using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class DireWolf : Enemy
{
    public override void MakePMatrix()
    {
        ProbabiltyMatrix = new float[] {.8f, .0f,.2f};
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
