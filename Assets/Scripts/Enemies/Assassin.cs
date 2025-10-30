using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Enemy
{
    public override void MakePMatrix()
    {
        ProbabiltyMatrix = new float[] { .6f, .0f, .4f };
        base.MakePMatrix();
    }

    public override void SpecialAttack()
    {
        //poison player
        PlayerManager.instance.ChangeStatus(PlayerManager.Status.poisoned);
    }
}
