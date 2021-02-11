using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerManager playerManager;

    [SerializeField] private GameObject winGameScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject videoTape;
    [SerializeField] private GameObject exit;
    [SerializeField] private int numberOfEnemiesToSpawn;

    [SerializeField] private List<Vector3> legalSpawnPositions;
    private Vector3 playerSpawnPosition;
    private bool isPaused = false;
    public UnityEvent OnGameReady;

    private void Awake()
    {
        if(OnGameReady == null) OnGameReady = new UnityEvent();
    }

    private void Start()
    {
        loadingScreen.SetActive(true);
        LoadLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }

    private void LoadLevel()
    {
        levelGenerator.GenerateLevel();
    }

    private void PauseMenu()
    {
        isPaused = !isPaused;
        Cursor.visible = isPaused;
        pauseScreen.SetActive(isPaused);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void BeginGame()
    {
        // for now, buffers and num of enemies are manually set but eventually it will be dictated by a room class
        GenerateLegalSpawnPositions();
        SpawnItem(videoTape);
        SpawnEnemies(numberOfEnemiesToSpawn);
        SpawnPlayer();
        DoneLoadingLevel();
    }

    private void GenerateLegalSpawnPositions()
    {
        List<Vector3> availableFloorPositions = levelGenerator.GetFloorPositions;
        playerSpawnPosition = levelGenerator.GetPlayerSpawnPoint;
        legalSpawnPositions = new List<Vector3>();
        foreach (var position in availableFloorPositions)
        {
            var direction = (playerSpawnPosition - position).magnitude;
            if (direction > 25f)
            {
                legalSpawnPositions.Add(position);
            }
        }
    }

    private void SpawnItem(GameObject itemToSpawn)
    {
        var randomIndex = Random.Range(0, legalSpawnPositions.Count);
        var randomPosition = legalSpawnPositions[randomIndex];
        var minDistance = levelGenerator.GetRoomWidth / 2;
        
        while ((playerSpawnPosition - randomPosition).magnitude < minDistance)
        {
            randomIndex = Random.Range(0, legalSpawnPositions.Count);
            randomPosition = legalSpawnPositions[randomIndex];
        }
        
        legalSpawnPositions.Remove(randomPosition);
        Instantiate(itemToSpawn, randomPosition, quaternion.identity);
    }

    private void SpawnObjective(GameObject objectiveToSpawn)
    {
        var randomIndex = Random.Range(0, legalSpawnPositions.Count);
        var randomPosition = legalSpawnPositions[randomIndex];
        var minDistance = levelGenerator.GetRoomWidth / 2;
        
        while ((playerSpawnPosition - randomPosition).magnitude < minDistance)
        {
            randomIndex = Random.Range(0, legalSpawnPositions.Count);
            randomPosition = legalSpawnPositions[randomIndex];
        }
        
        legalSpawnPositions.Remove(randomPosition);
        GameObject exitObj = Instantiate(objectiveToSpawn, randomPosition, quaternion.identity);
        playerManager.TrackObjective(exitObj);
    }
    
    private void SpawnEnemies(int numberToSpawn)
    {
        if (numberOfEnemiesToSpawn > 0)
        {
            List<Vector3> enemySpawnPositions  = new List<Vector3>();
            var safePosition = false;
            var position = Vector3.zero;
            for (int i = 0; i < numberToSpawn; i++)
            {
                var iterations = 0;

                // do this action until you find a safe spawn point or until you reach max iterations and skip the enemy
                do
                {
                    // assume the next iteration will yield a safe position
                    safePosition = true;
                
                    // get a random position
                    position = GetRandomPosition();
                
                    // check each presviously used position to make sure they are outside of the 5 unity radius
                    foreach (Vector3 usedPositions in enemySpawnPositions)
                    {
                        // if any of these are within the safety radius, flag as unsafe and iterated again
                        if ((usedPositions - position).magnitude < 5)
                        {
                            safePosition = false;
                        }
                    }

                    iterations++;
                
                    // if iterations reach 100 skip the enemy spawn
                    // if we found a safe spot instead, add position to list and spawn enemy
                } while (iterations < 100 && !safePosition);

                enemySpawnPositions.Add(position);
            }
        
            enemySpawner.SpawnAnEnemy(enemySpawnPositions);
        }
        
    }

    private Vector3 GetRandomPosition()
    {
        var randomIndex = Random.Range(0, legalSpawnPositions.Count);
        var randomPosition = legalSpawnPositions[randomIndex];

        return randomPosition;
    }

    private void SpawnPlayer()
    {
        GameObject obj = Instantiate(player, levelGenerator.GetPlayerSpawnPoint, Quaternion.identity);
        playerManager = obj.GetComponent<PlayerManager>();
    }
    
    private void DoneLoadingLevel()
    {
        loadingScreen.SetActive(false);  
        OnGameReady.Invoke();
    }

    public void LevelCleared()
    {
        SpawnObjective(exit);
        playerManager.SendMessageToMessageSystem("Get to the exit.", 2);
    }
}
