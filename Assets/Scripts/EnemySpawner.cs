using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private TextMeshProUGUI enemiesAliveText;
    public UnityEvent OnAllEnemiesDead;
    private int numberOfEnemiesSpawned = 0;
    private int numberOfEnemiesAlive = 0;

    private void Awake()
    {
        if(OnAllEnemiesDead == null) OnAllEnemiesDead = new UnityEvent();
    }

    public void SpawnAnEnemy(List<Vector3> atPositions)
    {
        foreach (Vector3 position in atPositions)
        {
            Instantiate(enemyPrefab, position, quaternion.identity);
            numberOfEnemiesSpawned++;
        }

        numberOfEnemiesAlive = numberOfEnemiesSpawned;
        enemiesAliveText.text = "Enemies Alive: " + numberOfEnemiesAlive;
    }

    public void EnemyDead()
    {
        numberOfEnemiesAlive--;
        enemiesAliveText.text = "Enemies Alive: " + numberOfEnemiesAlive;
        if (numberOfEnemiesAlive <= 0)
        {
            OnAllEnemiesDead.Invoke();
            enemiesAliveText.text = "Enemies Alive: None";
        }
    }
}
