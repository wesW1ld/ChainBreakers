using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Skeleton : Enemy
{
    public override void MakePMatrix()
    {
        ProbabiltyMatrix = new float[] {.2f, .0f,.8f};
        base.MakePMatrix();
    }

    public override void SpecialAttack(int i)
    {
        //bleed the player
        PlayerManager.instance.ChangeStatus(PlayerManager.Status.poisoned);
    }

    public override void SpecialAttackPreview(int i)
    {
        base.textUI.text = "You have been poisoned!";
    }

}
