using System.Collections;
using System.Collections.Generic;
using Ash.StateMachine;
using UnityEngine;

public class EnemyIdleState : State<EnemyHandler>
{
    public override void EnterState(EnemyHandler enemy)
    {
        //Debug.Log("Enemy Idle State.");
    }

    public override void ExitState(EnemyHandler enemy)
    {
    }

    public override void UpdateState(EnemyHandler enemy)
    {
        //var dist = enemy.CheckDistanceToPlayer();
        //
        //// check if player is in enemies radius
        //if (dist <= enemy.GetMaxRadius)
        //{
        //    // can the player be seen?
        //    if (enemy.CheckLineOfSightToPlayer())
        //    {
        //        // is the player in shoot range?
        //        if (dist < enemy.GetMinRadius)
        //        {
        //            // shoot
        //            enemy.stateMachine.ChangeState(enemy.enemyShootState);
        //        }
        //        else 
        //        {
        //            // move into shoot range
        //            enemy.stateMachine.ChangeState(enemy.enemyMoveState);
        //        }
        //    }
        //    else
        //    {
        //        // stay in idle
        //    }
        //}
    }

    public override void FixedUpdateState(EnemyHandler enemy)
    {
    }
}
