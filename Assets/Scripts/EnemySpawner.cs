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
    [SerializeField] private int numberOfEnemiesToSpawn;
    [SerializeField] private float timeBetweenSpawn;
    [SerializeField] private bool canSpawn = false;

    public UnityEvent OnEnemyDead;
    //private int[] options;
    private int aliveEnemies;
    private int enemyModifier = 0;

    private void Awake()
    {
        //options = new[] {-1, 1};
        if(OnEnemyDead == null) OnEnemyDead = new UnityEvent();
    }

    private void Start()
    {
        aliveEnemies = numberOfEnemiesToSpawn;
        canSpawn = true;
    }

    private void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;
            StartCoroutine(SpawnEnemies());
        }

        if (aliveEnemies <= 0)
        {
            AdjustEnemyCount();
            Invoke("SetCanSpawn", 1f);
        }
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < aliveEnemies; i++)
        {
            var pos = GetSpawnPosition();
            Instantiate(enemyPrefab, pos, quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }

    private void SetCanSpawn()
    {
        canSpawn = true;
    }

    private Vector2 GetSpawnPosition()
    {
        //var screenBounds = GetScreenBounds();

        //var xAxis = Random.Range(0, options.Length);
        //var yAxis = Random.Range(0, options.Length);
        var xRange = Random.Range(-8f, 8f);
        var yRange = Random.Range(-8f, 8f);
        //var randomPosition = new Vector2(8 * options[xAxis], 8 * options[yAxis]);
        var randomPosition = new Vector2(xRange, yRange);
        return randomPosition;
    }

    private Vector2 GetScreenBounds()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        var screenBounds = new Vector2((width / 2) + 4, (height / 2) + 2);

        return screenBounds;
    }

    private void AdjustEnemyCount()
    {
        enemyModifier++;
        aliveEnemies = numberOfEnemiesToSpawn + enemyModifier;
    }

    public void EnemyDead()
    {
        aliveEnemies--;
        OnEnemyDead.Invoke();
    }
}
