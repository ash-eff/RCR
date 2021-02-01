using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTile : LevelTile
{
    [SerializeField] private BoxCollider2D collider2D;
    [SerializeField] private SpriteRenderer topSprite;
    public Sprite[] availableTopSprites;
    public Sprite[] availableBotSprites;

    public bool upNeighbor;
    public bool rightNeighbor;
    public bool downNeighbor;
    public bool leftNeighbor;
    public Vector2 upTile;
    public Vector2 rightTile;
    public Vector2 downTile;
    public Vector2 leftTile;
    protected override void CheckNeighbors()
    {
        var startingPos = new Vector2((int)gridPos.x, (int)gridPos.y);
        
        upTile = startingPos + Vector2.up;
        rightTile = startingPos + Vector2.right;
        downTile = startingPos + Vector2.down;
        leftTile = startingPos + Vector2.left;

        if (upTile.y <= levelGenerator.roomHeight - 1)
        {
            if (levelGenerator.grid[(int) upTile.x, (int) upTile.y] == LevelGenerator.GridSpace.Floor)
            {
                upNeighbor = false;
                collider2D.offset = new Vector2(0, -.25f);
                collider2D.size = new Vector2(1, .5f);
            }
        }
        else
        {
            collider2D.offset = new Vector2(0, -.25f);
            collider2D.size = new Vector2(1, .5f);
        }

        if (rightTile.x <= levelGenerator.roomWidth - 1)
        {
            if (levelGenerator.grid[(int) rightTile.x, (int) rightTile.y] == LevelGenerator.GridSpace.Floor)
            {
                rightNeighbor = false;
            }
        }

        if (downTile.y >= 0)
        {
            if (levelGenerator.grid[(int) downTile.x, (int) downTile.y] == LevelGenerator.GridSpace.Floor)
            {
                downNeighbor = false;
            }
        }

        if (leftTile.x >= 0)
        {
            if (levelGenerator.grid[(int) leftTile.x, (int) leftTile.y] == LevelGenerator.GridSpace.Floor)
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
            bottomSprite.sprite = availableBotSprites[4];
            topSprite.sprite = availableTopSprites[4];

        }
        
        else if (upNeighbor && rightNeighbor && downNeighbor)
        {
            // middle left 3
            bottomSprite.sprite = availableBotSprites[3];
            topSprite.sprite = availableTopSprites[3];

        }
            
        else if (rightNeighbor && downNeighbor && leftNeighbor)
        {
            // top center 1
            bottomSprite.sprite = availableBotSprites[1];
            topSprite.sprite = availableTopSprites[1];

        }
        
        else if (downNeighbor && leftNeighbor && upNeighbor)
        {
            // middle right 5
            bottomSprite.sprite = availableBotSprites[5];
            topSprite.sprite = availableTopSprites[5];

        }
        
        else if (leftNeighbor && upNeighbor && rightNeighbor)
        {
            // bottom center 7
            bottomSprite.sprite = availableBotSprites[7];
            topSprite.sprite = availableTopSprites[7];

        }
        
        else if (upNeighbor && downNeighbor)
        {
            // tall center 10
            bottomSprite.sprite = availableBotSprites[10];
            topSprite.sprite = availableTopSprites[10];

        }
        
        else if (upNeighbor && rightNeighbor)
        {
            // bottom left 6
            bottomSprite.sprite = availableBotSprites[6];
            topSprite.sprite = availableTopSprites[6];

        }
        
        else if (upNeighbor && leftNeighbor)
        {
            // bottom right 8
            bottomSprite.sprite = availableBotSprites[8];
            topSprite.sprite = availableTopSprites[8];
        }
        
        else if (rightNeighbor && leftNeighbor)
        {
            // wide center 13
            bottomSprite.sprite = availableBotSprites[13];
            topSprite.sprite = availableTopSprites[13];
        }
        
        else if (rightNeighbor && downNeighbor)
        {
            // top left 0
            bottomSprite.sprite = availableBotSprites[0];
            topSprite.sprite = availableTopSprites[0];
        }
        
        else if (leftNeighbor && downNeighbor)
        {
            // top right 2
            bottomSprite.sprite = availableBotSprites[2];
            topSprite.sprite = availableTopSprites[2];
        }
        
        else if (upNeighbor)
        {
            // tall bottom 9
            bottomSprite.sprite = availableBotSprites[9];
            topSprite.sprite = availableTopSprites[9];
        }

        else if (downNeighbor)
        {
            // tall top 11
            bottomSprite.sprite = availableBotSprites[11];
            topSprite.sprite = availableTopSprites[11];
        }
        
        else if (rightNeighbor)
        {
            // wide left 12
            bottomSprite.sprite = availableBotSprites[12];
            topSprite.sprite = availableTopSprites[12];
        }

        else
        {
            // wide right 12
            bottomSprite.sprite = availableBotSprites[14];
            topSprite.sprite = availableTopSprites[14];
        }
    }
}
