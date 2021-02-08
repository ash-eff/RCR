using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

//[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
    public enum GridSpace
    {
        Empty,
        Floor,
        Wall,
        Obstacle,
        Center
    };
 
    [SerializeField] private Vector2 roomSizeWorldUnits = new Vector2(30, 30);
    [SerializeField] private Vector2 worldUnitsInOneGridCell = new Vector2(1f, 1.44f);
    [UnityEngine.Range(0.25f, 1.0f)][SerializeField] private float chanceWalkerChangeDir = 0.6f;
    [SerializeField] private GameObject wallObj, floorObj, emptyObj, obstacleObj;

    public UnityEvent OnLevelLoaded;
    
    struct Walker
    {
        public Vector3 dir;
        public Vector3 pos;
    }
    

    private List<Walker> walkers;
    private List<GameObject> tiles = new List<GameObject>();
    //public List<Vector2> gridPositions = new List<Vector2>();
    private List<Vector3> floorPositions = new List<Vector3>();
    private GridSpace[,] grid;
    private Vector3 playerSpawnPoint;
    
    private float chanceWalkerSpawn = 0.05f;
    private float chanceWalkerDestroy = 0.05f;
    private float percentToFill = 0.2f;

    private int roomHeight, roomWidth;
    private int maxWalkers = 10;

    public GridSpace[,] GetGrid => grid;
    public int GetRoomHeight => roomHeight;
    public int GetRoomWidth => roomWidth;
    public Vector3 GetPlayerSpawnPoint => playerSpawnPoint;
    public List<Vector3> GetFloorPositions => floorPositions;

    private void Awake()
    {
        if(OnLevelLoaded == null) OnLevelLoaded = new UnityEvent();
    }

    public void GenerateLevel()
    {
        ResetLevel();
        SetUp();
        CreateFloors();
        CreateWalls();
        //DealWithSingleWalls();
        DealWithSingleRowsOfWalls();
        GenerateListOfFloorPositions();
        SetPlayerSpawnPoint();
        // SetEnemySpawnPoints();
        // SetItemSpawnPoints();
        SpawnLevel();
        
    }

    public void ResetLevel()
    {
        if (tiles.Count > 0)
        {
            foreach (GameObject tile in tiles)
            {
                DestroyImmediate(tile);
            }
            
            //gridPositions.Clear();
            tiles.Clear();
            floorPositions.Clear();
        }
        
        //tiles = new List<GameObject>();
        //gridPositions = new List<Vector2>();
        //floorPositions = new List<Vector3>();
    }

    private void SetUp()
    {
        // find grid size
        roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.x);
        roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.y);

        // create grid
        grid = new GridSpace[roomWidth, roomHeight];
        
        // set grid's default state
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                grid[x, y] = GridSpace.Empty; // make every cell empty
                //gridPositions.Add(new Vector2(x, y ));
            }
        }

        // set first walker
        walkers = new List<Walker>();
        Walker newWalker = new Walker();
        newWalker.dir = RandomDirection();
        Vector3 spawnPos = new Vector3(Mathf.RoundToInt(roomWidth / 2.0f), 0f, Mathf.RoundToInt(roomHeight / 2.0f));        
        newWalker.pos = spawnPos;
        walkers.Add(newWalker);
    }

    private void CreateFloors()
    {
        int iterations = 0;
        do
        {
            // create floor at every position of walker
            foreach (Walker myWalker in walkers)
            {
               // Debug.Log("X: " + myWalker.pos.x + " Y: " + myWalker.pos.z);
                grid[(int) myWalker.pos.x, (int) myWalker.pos.z] = GridSpace.Floor;

                // as long as placing a floor a space above the walker is in range, do it
                if(myWalker.pos.z + 1 < roomHeight - 2)
                   grid[(int) myWalker.pos.x, (int) myWalker.pos.z + 1] = GridSpace.Floor;
                
                //// as long as placing a floor a space to the right of the walker is in range, do it
                if(myWalker.pos.x + 1 < roomWidth - 2)
                    grid[(int) myWalker.pos.x + 1, (int) myWalker.pos.z] = GridSpace.Floor;
            }
                
            
            // chance: destroy a walker
            int numberChecks = walkers.Count;
            for(int i = 0; i < numberChecks; i++)
                if (Random.value < chanceWalkerDestroy && walkers.Count > 1)
                {
                    walkers.RemoveAt(i);
                    break; // only destroy one walker per iteration
                }
            
            // chance: walker pick a new direction
            for(int i = 0; i < walkers.Count; i++)
                if (Random.value < chanceWalkerChangeDir)
                {
                    Walker thisWalker = walkers[i];
                    thisWalker.dir = RandomDirection();
                    walkers[i] = thisWalker;
                }
            
            // chance: spawn a new walker
            numberChecks = walkers.Count;
            for(int i = 0; i < numberChecks; i ++)
                if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers)
                {
                    // create a walker
                    Walker newWalker = new Walker();
                    newWalker.dir = RandomDirection();
                    newWalker.pos = walkers[i].pos;
                    walkers.Add(newWalker);
                }
            
            // move walkers
            for (int i = 0; i < walkers.Count; i++)
            {
                Walker thisWalker = walkers[i];
                thisWalker.pos += thisWalker.dir;
                walkers[i] = thisWalker;
            }
                
            // avoid border of the grid
            for (int i = 0; i < walkers.Count; i++)
            {
                Walker thisWalker = walkers[i];
                // clamp x,y to leave a 1 space border: leave room for walls
                thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, roomWidth - 2);
                thisWalker.pos.z = Mathf.Clamp(thisWalker.pos.z, 1, roomWidth - 2);
                walkers[i] = thisWalker;
            }
            
            // check to exit loop
            if ((float) NumberOfFloors() / (float) grid.Length > percentToFill)
                break;
            
            iterations++;
            
        } while (iterations < 100000);
    }

    private void CreateWalls()
    {
        // loop through every grid space
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                // if there is a floor, check neighboring spaces
                if (grid[x, y] == GridSpace.Floor)
                {
                    //if any surrounding spaces are empty, place a wall
                    if (grid[x,y+1] == GridSpace.Empty)
                        grid[x,y+1] = GridSpace.Wall;
                    if (grid[x,y-1] == GridSpace.Empty)
                        grid[x,y-1] = GridSpace.Wall;
                    if (grid[x+1,y] == GridSpace.Empty)
                        grid[x+1,y] = GridSpace.Wall;
                    if (grid[x-1,y] == GridSpace.Empty)
                        grid[x-1,y] = GridSpace.Wall;
                }
            }
        }
    }
    
    private void DealWithSingleWalls(){
        //loop though every grid space
        for (int x = 0; x < roomWidth-1; x++)
        {
            for (int y = 0; y < roomHeight-1; y++)
            {
                //if theres a wall, check the spaces around it
                if (grid[x,y] == GridSpace.Wall)
                {
                    //assume all space around wall are floors
                    bool allFloors = true;
                    //check each side to see if they are all floors
                    for (int checkX = -1; checkX <= 1 ; checkX++)
                    {
                        for (int checkY = -1; checkY <= 1; checkY++)
                        {
                            if (x + checkX < 0 || x + checkX > roomWidth - 1 || 
                                y + checkY < 0 || y + checkY > roomHeight - 1)
                            {
                                //skip checks that are out of range
                                continue;
                            }
                            if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0))
                            {
                                //skip corners and center
                                continue;
                            }
                            if (grid[x + checkX,y+checkY] != GridSpace.Floor)
                            {
                                allFloors = false;
                            }
                        }
                    }
                    if (allFloors)
                    {
                        grid[x,y] = GridSpace.Floor;
                    }
                }
            }
        }
    }
    
    private void DealWithSingleRowsOfWalls()
    {
        //loop though every grid space
        for (int x = 0; x < roomWidth-1; x++)
        {
            for (int y = 0; y < roomHeight-1; y++)
            {
                //if theres a wall, check the spaces above and below it
                if (grid[x,y] == GridSpace.Wall)
                {
                    //assume both spaces above and below wall are floors
                    bool aboveIsFloors = true;
                    bool belowIsFloor = true;
                    
                    // as long as space above and below are in range
                    if (y + -1 >= 0  && y + 1 <= roomHeight - 1)
                    {
                        if (grid[x, y + 1] != GridSpace.Floor)
                        {
                            aboveIsFloors = false;
                        }
                        
                        if (grid[x, y - 1] != GridSpace.Floor)
                        {
                            aboveIsFloors = false;
                        }
                    }

                    // if both above and below are floors, set the above floor to a wall
                    if (aboveIsFloors && belowIsFloor)
                    {
                        grid[x, y + 1] = GridSpace.Wall;
                    }
                }
            }
        }
    }

    private void SetPlayerSpawnPoint()
    {
        playerSpawnPoint = floorPositions[0];
    }

    private void SpawnLevel()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                switch (grid[x,y])
                {
                    case GridSpace.Empty:
                        Spawn(x, y, emptyObj);
                        break;
                    case GridSpace.Floor:
                        Spawn(x, y, floorObj);
                        break;
                    case GridSpace.Wall:
                        Spawn(x, y, wallObj);
                        break;
                    case GridSpace.Obstacle:
                        Spawn(x, y, obstacleObj);
                        break;
                }
            }
        }
        
        OnLevelLoaded.Invoke();
        //enabled = false;
    }

    private void GenerateListOfFloorPositions()
    {
        Vector3 offset = new Vector3(roomSizeWorldUnits.x / 2f, 0f, roomSizeWorldUnits.y / 2f);
        
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                if (grid[x, y] == GridSpace.Floor)
                {
                    Vector3 floorWorldPosition = new Vector3(x * worldUnitsInOneGridCell.x,0f,y * worldUnitsInOneGridCell.y) - offset;
                    floorPositions.Add(floorWorldPosition);
                }
            }
        }
    }

    private Vector3 RandomDirection()
    {
        // pick int between 0 and 3
        int choice = Mathf.FloorToInt(Random.value * 3.99f);
        // use that int to choose a direction
        switch (choice)
        {
            case 0:
                return Vector3.back;
            case 1:
                return Vector3.left;
            case 2:
                return Vector3.forward;
            default:
                return Vector3.right;
        }
    }

    private int NumberOfFloors()
    {
        int count = 0;
        foreach (GridSpace space in grid)
            if (space == GridSpace.Floor)
                count++;

        return count;
    }

    private void Spawn(float x, float y, GameObject toSpawn)
    {
        // find position to spawn
        Vector3 offset = new Vector3(roomSizeWorldUnits.x / 2f, 0f, roomSizeWorldUnits.y / 2f);
        Vector3 spawnPos = new Vector3(x * worldUnitsInOneGridCell.x,0,y * worldUnitsInOneGridCell.y) - offset;
        // spawn object
        GameObject obj = Instantiate(toSpawn, spawnPos, Quaternion.identity);
        LevelTile lt = obj.GetComponent<LevelTile>();
        if(lt != null)
            lt.SetGridPos = new Vector2(x,y);
        obj.gameObject.transform.parent = this.transform;
        tiles.Add(obj);
    }
}
