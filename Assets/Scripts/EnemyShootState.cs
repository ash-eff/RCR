using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ash.MyUtils;
using Ash.StateMachine;
using UnityEngine;

public class EnemyShootState : State<EnemyHandler>
{
    private Vector2 direction;
    private float rot;
    
    public override void EnterState(EnemyHandler enemy)
    {
        Debug.Log("Enemy Shoot State.");
        enemy.IsShooting = true;
    }

    public override void ExitState(EnemyHandler enemy)
    {
    }

    public override void UpdateState(EnemyHandler enemy)
    {
       // if (enemy.IsShooting && Time.time > enemy.GetRateOfFire + enemy.GetLastShot && enemy.GetAmmoCount > 0)
       // {
       //     direction = enemy.GetPlayerPosition - enemy.transform.position;
       //     rot = MyUtils.GetAngleFromVectorFloat(direction.normalized);
       //     enemy.FireWeapon(direction, rot);
       // }
       // else if(enemy.IsShooting && enemy.GetAmmoCount <= 0)
       // { 
       //     enemy.Reload();
       //     enemy.IsShooting = false;
       // }
       // 
       //// complete shooting
       //if (!enemy.IsShooting && !enemy.IsReloading)
       //{
       //    // if the player still in view
       //    if (enemy.CheckLineOfSightToPlayer())
       //    {
       //        // if the player is in shoot range
       //        var dist = enemy.CheckDistanceToPlayer();
       //        if (dist < enemy.GetMinRadius)
       //        {
       //            // shoot again
       //            direction = enemy.GetPlayerPosition - enemy.transform.position;
       //            rot = MyUtils.GetAngleFromVectorFloat(direction.normalized);
       //            enemy.IsShooting = true;
       //        }
       //        else
       //        {
       //            // move to shoot range  
       //            enemy.stateMachine.ChangeState(enemy.enemyMoveState);
       //        }
       //    }
       //    else
       //    {
       //        // idle
       //    }
       //}

    }

    public override void FixedUpdateState(EnemyHandler enemy)
    {
    }
}
