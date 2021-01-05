using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ash.MyUtils;
using Ash.StateMachine;
using UnityEngine;

public class EnemyShootState : State<EnemyHandler>
{
    public override void EnterState(EnemyHandler enemy)
    {
        Debug.Log("Enemy Shoot State.");
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
            var direction = enemy.GetPlayerPosition - enemy.transform.position;
            var rot = MyUtils.GetAngleFromVectorFloat(direction.normalized);
            
            if (Time.time > enemy.GetRateOfFire + enemy.GetLastShot && enemy.GetAmmoCount > 0)
            {
                enemy.FireWeapon(direction, rot);
            }

            if (enemy.GetAmmoCount == 0 && !enemy.IsReloading)
            {
                enemy.Reload();
            }

            if (enemy.CheckLineOfSightToPlayer())
            {
                
            }
            else
            {
                enemy.stateMachine.ChangeState(enemy.enemyIdleState);
            }
        }
        // if player leaves radius then idle
        else
        {
            enemy.stateMachine.ChangeState(enemy.enemyIdleState);
        }
    }

    public override void FixedUpdateState(EnemyHandler enemy)
    {
    }
}
