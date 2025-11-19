using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    public override void MakePMatrix()
    {
        ProbabiltyMatrix = new float[] { .9f, .1f, .0f };
        base.MakePMatrix();
    }
}
