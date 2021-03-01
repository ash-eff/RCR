using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ash.MyUtils;
using Ash.StateMachine;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EnemyMoveState : State<Enemy>
{
    private Vector3 targetPosition;
    
    public override void EnterState(Enemy enemy)
    {
    }

    public override void ExitState(Enemy enemy)
    {
    }

    public override void UpdateState(Enemy enemy)
    {
        if (enemy.IsPlayerIsInRange())
        {
            if (enemy.IsPlayerInAttackRange())
                enemy.Attack();
            else
                enemy.MoveToPosition(enemy.PlayerLocation);
        }
        else
        {
            enemy.ReactToPlayerLeavingRange();
        }
    }

    public override void FixedUpdateState(Enemy enemy)
    {
    }
}
