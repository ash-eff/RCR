using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAcidBug : Enemy
{
    public GameObject acidExplosion;
    
    public override void ReactToPlayerInRange()
    {
        SwapState(enemyMoveState);
    }

    public override void Attack()
    {
        EnemyExplode();
    }

    public override void EnemyExplode()
    {
        GameObject obj = Instantiate(acidExplosion, transform.position, Quaternion.identity);
        Destroy(obj, 2f);
        SwapState(enemyDeathState);
    }

    public override void EnemyDeath()
    {
        Destroy(gameObject);
    }
}
