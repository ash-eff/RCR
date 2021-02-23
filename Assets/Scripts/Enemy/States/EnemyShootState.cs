using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootState : State<Enemy>
{
    //private Vector2 direction;
    //private float rot;
    private BulletPatterns bp;
    
    public override void EnterState(Enemy enemy)
    {
        bp = enemy.bulletPatterns;
        bp.ThreeSixtySpread();
        //Debug.Log("Enemy Shoot State.");
        //enemy.IsShooting = true;
    }

    public override void ExitState(Enemy enemy)
    {
        bp.StopShooting();
    }

    public override void UpdateState(Enemy enemy)
    {
        if (!enemy.IsPlayerIsInRange())
        {
            if(!bp.isShooting)
                enemy.ReactToPlayerLeavingRange();
        }
    }

    public override void FixedUpdateState(Enemy enemy)
    {
    }
}
