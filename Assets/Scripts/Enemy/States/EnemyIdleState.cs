using System.Collections;
using System.Collections.Generic;
using Ash.StateMachine;
using UnityEngine;

public class EnemyIdleState : State<Enemy>
{
    public override void EnterState(Enemy enemy)
    {
        enemy.agent.isStopped = true;
    }

    public override void ExitState(Enemy enemy)
    {
        enemy.agent.isStopped = false;
    }

    public override void UpdateState(Enemy enemy)
    {
        if (enemy.IsPlayerIsInRange())
        {
            if (enemy.IsPlayerInLineOfSight())
            {
                enemy.ReactToPlayerInRange();
            }
        }
    }

    public override void FixedUpdateState(Enemy enemy)
    {
    }
}
