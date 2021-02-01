using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : LevelTile
{
    public Sprite[] availableSprites;
    public bool hasWallAbove = false;
    protected override void CheckNeighbors()
    {
        var startingPos = new Vector2((int)gridPos.x, (int)gridPos.y);
        var aboveTile = startingPos + Vector2.up;
        if (levelGenerator.grid[(int) aboveTile.x, (int) aboveTile.y] == LevelGenerator.GridSpace.Wall)
        {
            hasWallAbove = true;
        }
    }

    protected override void AssignSprite()
    {
        if (hasWallAbove)
            bottomSprite.sprite = availableSprites[0];
        else
            bottomSprite.sprite = availableSprites[Random.Range(1, availableSprites.Length - 1)];
    }
}