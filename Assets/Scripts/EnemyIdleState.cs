using System.Collections;
using System.Collections.Generic;
using Ash.StateMachine;
using UnityEngine;

public class EnemyIdleState : State<EnemyHandler>
{
    public override void EnterState(EnemyHandler enemy)
    {
        Debug.Log("Enemy Idle State.");
    }

    public override void ExitState(EnemyHandler enemy)
    {
    }

    public override void UpdateState(EnemyHandler enemy)
    {
        var dist = enemy.CheckDistanceToPlayer();
        // check if player is in enemies radius
        if (dist <= enemy.GetMaxRadius)
        {
            // enemy needs to move if player outside of min radius
            
            // and shoot if inside of min radius
            enemy.stateMachine.ChangeState(enemy.enemyShootState);
            
            // also need to check if player is in line of sight
        }
        
        // while no player is in radius or line of sight
        // move to a valid random spot close by and wait a few seconds
    }

    public override void FixedUpdateState(EnemyHandler enemy)
    {
    }
}
