using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
    public enum GridSpace
    {
        Empty,
        Floor,
        Wall,
        Obstacle
    };

    public GridSpace[,] grid;
    public int roomHeight, roomWidth;
    [SerializeField] private Vector2 roomSizeWorldUnits = new Vector2(30, 30);
    [SerializeField] private float worldUnitsInOneGridCell = 1;
    struct Walker
    {
        public Vector2 dir;
        public Vector2 pos;
    }

    private List<Walker> walkers;
    private List<GameObject> tiles = new List<GameObject>();
    [UnityEngine.Range(0.25f, 1.0f)][SerializeField] private float chanceWalkerChangeDir = 0.5f;
    private float chanceWalkerSpawn = 0.05f;
    private float chanceWalkerDestroy = 0.05f;
    private int maxWalkers = 10;
    private float percentToFill = 0.2f;
    [SerializeField] private GameObject wallObj, floorObj, emptyObj, obstacleObj;

    //private void Start()
    //{
    //    SetUp();
    //    CreateFloors();
    //    CreateWalls();
    //    RemoveSingleWalls();
    //    SpawnLevel();
    //}
    
    public void GenerateLevel()
    {
        ResetLevel();
        SetUp();
        CreateFloors();
        CreateWalls();
        RemoveSingleWalls();
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
            
            tiles.Clear();
        }
        
        tiles = new List<GameObject>();
    }

    private void SetUp()
    {
        // find grid size
        roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.x / worldUnitsInOneGridCell);
        roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.y / worldUnitsInOneGridCell);
        
        // create grid
        grid = new GridSpace[roomWidth, roomHeight];
        
        // set grid's default state
        for(int x = 0; x < roomWidth - 1; x++)
            for (int y = 0; y < roomHeight - 1; y++)
                grid[x, y] = GridSpace.Empty; // make every cell empty

        // set first walker
        walkers = new List<Walker>();
        Walker newWalker = new Walker();
        newWalker.dir = RandomDirection();
        Vector2 spawnPos = new Vector2(Mathf.RoundToInt(roomWidth / 2.0f), Mathf.RoundToInt(roomHeight / 2.0f));
        newWalker.pos = spawnPos;
        walkers.Add(newWalker);
    }

    private void CreateFloors()
    {
        int interations = 0;
        do
        {
            // create floor at every position of walker
            foreach (Walker myWalker in walkers)
                grid[(int) myWalker.pos.x, (int) myWalker.pos.y] = GridSpace.Floor;
            
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
                thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, roomWidth - 2);
                walkers[i] = thisWalker;
            }
            
            // check to exit loop
            if ((float) NumberOfFloors() / (float) grid.Length > percentToFill)
                break;
            
            interations++;
            
        } while (interations < 100000);
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
    
    private void RemoveSingleWalls(){
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
                        grid[x,y] = GridSpace.Obstacle;
                    }
                }
            }
        }
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
    }

    private Vector2 RandomDirection()
    {
        // pick int between 0 and 3
        int choice = Mathf.FloorToInt(Random.value * 3.99f);
        // use that int to choose a direction
        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            default:
                return Vector2.right;
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
        Vector2 offset = roomSizeWorldUnits / 2.0f;
        Vector2 spawnPos = new Vector2(x,y) * worldUnitsInOneGridCell - offset;
        // spawn object
        GameObject obj = Instantiate(toSpawn, spawnPos, Quaternion.identity);
        LevelTile lt = obj.GetComponent<LevelTile>();
        if(lt != null)
            lt.SetGridPos = new Vector2(x,y);
        obj.gameObject.transform.parent = this.transform;
        tiles.Add(obj);
    }
}
