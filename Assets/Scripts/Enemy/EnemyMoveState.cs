using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ash.MyUtils;
using Ash.StateMachine;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EnemyMoveState : State<EnemyHandler>
{
    //private Vector3 playerPosition;
    //private Vector3 dirToPlayer;
    private Vector3 targetPosition;
    //private bool isMoving;
    
    
    public override void EnterState(EnemyHandler enemy)
    {
        //Debug.Log("Enemy Move State.");
        //playerPosition = enemy.GetPlayerPosition;
        //dirToPlayer = playerPosition - enemy.transform.position;
        //targetPosition = enemy.transform.position + dirToPlayer.normalized * enemy.GetMinRadius;
        //isMoving = true;
    }

    public override void ExitState(EnemyHandler enemy)
    {
    }

    public override void UpdateState(EnemyHandler enemy)
    {
        //if (isMoving)
        //{
        targetPosition = enemy.GetPlayerPosition;
        float step =  enemy.moveSpeed * Time.deltaTime; // calculate distance to move
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPosition, step);
        //}

        //if (Vector3.Distance(enemy.transform.position, targetPosition) < .001f)
        //{
        //    isMoving = false;
        //}
//
        //if (!isMoving)
        //{
        //    var dist = enemy.CheckDistanceToPlayer();
        //
        //    // is the player in shoot range?
        //    if (dist <= enemy.GetMaxRadius)
        //    {
        //        // shoot
        //        enemy.stateMachine.ChangeState(enemy.enemyShootState);
        //    }
        //    else
        //    {
        //        enemy.stateMachine.ChangeState(enemy.enemyIdleState);
        //    }
        //}
    }

    public override void FixedUpdateState(EnemyHandler enemy)
    {

        
    }
}
