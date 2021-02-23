using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{
    public GameObject explosion;
    public Sprite reloadingSprite;
    public Sprite shootingSprite;

    public override void ReactToPlayerInRange()
    {
        Debug.Log("In Range");
        enemySprite.sprite = shootingSprite;
        SwapState(enemyShootState);
    }

    public override void ReactToPlayerLeavingRange()
    {
        Debug.Log("NOT In Range");
        enemySprite.sprite = reloadingSprite;
        SwapState(enemyIdleState);
    }

    public override void EnemyExplode()
    {
        bulletPatterns.StopShooting();
        Debug.Log("Boom!!!");
        GameObject obj = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(obj, 2f);
        SwapState(enemyDeathState);
    }

    public override void EnemyDeath()
    {
        Destroy(gameObject);
    }
}
