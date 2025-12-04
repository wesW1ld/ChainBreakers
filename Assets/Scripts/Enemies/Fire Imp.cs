using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class FireImp : Enemy
{
    public override void MakePMatrix()
    {
        ProbabiltyMatrix = new float[] {.5f, .0f,.5f};
        base.MakePMatrix();
    }

    public override void SpecialAttack(int i)
    {
        //bleed the player
        PlayerManager.instance.ChangeStatus(PlayerManager.Status.burning);
    }

    public override void SpecialAttackPreview(int i)
    {
        base.textUI.text = "You are burning!";
    }

}
