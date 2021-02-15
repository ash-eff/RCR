using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplodeState : State<Enemy>
{
    public override void EnterState(Enemy enemy)
    {
        enemy.EnemyExplode();
    }

    public override void ExitState(Enemy enemy)
    {
    }

    public override void UpdateState(Enemy enemy)
    {
    }

    public override void FixedUpdateState(Enemy enemy)
    {
    }
}
