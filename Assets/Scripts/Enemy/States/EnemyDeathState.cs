using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : State<Enemy>
{
    public override void EnterState(Enemy enemy)
    {
        enemy.EnemyDeath();
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

