using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy_ChaseAndExplode : Enemy
{
    public GameObject explosion;
    
    public override void ReactToPlayerInRange()
    {
        SwapState(enemyMoveState);
    }

    public override void EnemyExplode()
    {
        Debug.Log("Boom!!!");
        GameObject obj = Instantiate(explosion, transform.position, quaternion.identity);
        Destroy(obj, 2f);
        SwapState(enemyDeathState);
    }

    public override void ReactToPlayerLeavingRange()
    {
        SwapState(enemyIdleState);
    }
}
