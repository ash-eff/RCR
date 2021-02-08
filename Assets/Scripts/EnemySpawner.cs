using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    public void SpawnAnEnemy(List<Vector3> atPositions)
    {
        foreach (Vector3 position in atPositions)
        {
            Instantiate(enemyPrefab, position, quaternion.identity);
        }
    }
}
