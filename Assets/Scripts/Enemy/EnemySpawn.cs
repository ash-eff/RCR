using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    public BaseRoom spawnedFrom;
    
    public void SpawnEnemy()
    {
        GameObject enemyPrefab = Instantiate(enemy, transform.position, quaternion.identity);
        enemyPrefab.GetComponent<EnemyHandler>().spawnedFromRoom = spawnedFrom;
        Destroy(gameObject);
    }
}
