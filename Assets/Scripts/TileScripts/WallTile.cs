using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTile : LevelTile
{
    public bool upNeighbor;
    public bool rightNeighbor;
    public bool downNeighbor;
    public bool leftNeighbor;
    public Vector2 upTile;
    public Vector2 rightTile;
    public Vector2 downTile;
    public Vector2 leftTile;
    public BoxCollider boxCollider;
    
    private Vector3 defaultColliderSize = new Vector3(2f, 1.4f, 2.828f);
    private Vector3 topWallColliderSize = new Vector3(2f, 1.4f, 2.114f);
    private Vector3 topWallOffset = new Vector3(0, 0f, -.714f);
    private Vector3 bottomWallColliderSize = new Vector3(2f, 1.4f, 2.114f);
    private Vector3 bottomWallOffset = new Vector3(0, 0f, .714f);
    private Vector3 singleHorizontalRowWallColliderSize  = new Vector3(2f, 1.4f, 2.121f);
    private Vector3 singleWallOffset = new Vector3(0, 0f, -.3535f);

    
    protected override void CheckNeighbors()
    {
        var startingPos = new Vector2((int)gridPos.x, (int)gridPos.y);
        
        upTile = startingPos + Vector2.up;
        rightTile = startingPos + Vector2.right;
        downTile = startingPos + Vector2.down;
        leftTile = startingPos + Vector2.left;

        if (upTile.y <= levelGenerator.GetRoomHeight - 1)
        {
            if (levelGenerator.GetGrid[(int) upTile.x, (int) upTile.y] == LevelGenerator.GridSpace.Floor)
            {
                upNeighbor = false;
            }
        }

        if (rightTile.x <= levelGenerator.GetRoomWidth - 1)
        {
            if (levelGenerator.GetGrid[(int) rightTile.x, (int) rightTile.y] == LevelGenerator.GridSpace.Floor)
            {
                rightNeighbor = false;
            }
        }

        if (downTile.y >= 0)
        {
            if (levelGenerator.GetGrid[(int) downTile.x, (int) downTile.y] == LevelGenerator.GridSpace.Floor)
            {
                downNeighbor = false;
            }
        }

        if (leftTile.x >= 0)
        {
            if (levelGenerator.GetGrid[(int) leftTile.x, (int) leftTile.y] == LevelGenerator.GridSpace.Floor)
            {
                leftNeighbor = false;
            }
        }
    }

    protected override void AssignSprite()
    {
        if (upNeighbor && rightNeighbor && downNeighbor && leftNeighbor)
        {
            // middle  center 4
            tileSprite.sprite = availableSprites[4];

        }
        
        else if (upNeighbor && rightNeighbor && downNeighbor)
        {
            // middle left 3
            tileSprite.sprite = availableSprites[3];

        }
            
        else if (rightNeighbor && downNeighbor && leftNeighbor)
        {
            // top center 1
            tileSprite.sprite = availableSprites[1];
        }
        
        else if (downNeighbor && leftNeighbor && upNeighbor)
        {
            // middle right 5
            tileSprite.sprite = availableSprites[5];
        }
        
        else if (leftNeighbor && upNeighbor && rightNeighbor)
        {
            // bottom center 7
            tileSprite.sprite = availableSprites[7];
        }
        
        else if (upNeighbor && downNeighbor)
        {
            // tall center 10
            tileSprite.sprite = availableSprites[10];
        }
        
        else if (upNeighbor && rightNeighbor)
        {
            // bottom left 6
            tileSprite.sprite = availableSprites[6];
        }
        
        else if (upNeighbor && leftNeighbor)
        {
            // bottom right 8
            tileSprite.sprite = availableSprites[8]; 
        }
        
        else if (rightNeighbor && leftNeighbor)
        {
            // wide center 13
            tileSprite.sprite = availableSprites[13];
        }
        
        else if (rightNeighbor && downNeighbor)
        {
            // top left 0
            tileSprite.sprite = availableSprites[0];
        }
        
        else if (leftNeighbor && downNeighbor)
        {
            // top right 2
            tileSprite.sprite = availableSprites[2];
        }
        
        else if (upNeighbor)
        {
            // tall bottom 9
            tileSprite.sprite = availableSprites[9];
        }

        else if (downNeighbor)
        {
            // tall top 11
            tileSprite.sprite = availableSprites[11];
        }
        
        else if (rightNeighbor)
        {
            // wide left 12
            tileSprite.sprite = availableSprites[12];
        }

        else
        {
            // wide right 12
            tileSprite.sprite = availableSprites[14];
        }
    }
    
    protected override void SetColliderSize()
    {
        // if there is no top neighbors but has bottom neighbors

        if (!upNeighbor && downNeighbor)
        {
            boxCollider.center = singleWallOffset;
            boxCollider.size = singleHorizontalRowWallColliderSize;
        }
        
        // if there are no bottom neighbors but has top neighbors
        else if (upNeighbor && !downNeighbor) 
        {
            boxCollider.center = Vector3.zero;
            boxCollider.size = defaultColliderSize;
        }
        
        // if there are no top or bottom neighbors
        else if(!upNeighbor && !downNeighbor)

        {
            boxCollider.center = singleWallOffset;
            boxCollider.size = singleHorizontalRowWallColliderSize;
        }
        
        // default
        else
        {
            boxCollider.center = Vector3.zero;
            boxCollider.size = defaultColliderSize;
        }
    }
}
