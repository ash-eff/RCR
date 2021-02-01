using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTile : LevelTile
{
    public Sprite[] availableSprites;
    
    protected override void CheckNeighbors()
    {
    }

    protected override void AssignSprite()
    {
        bottomSprite.sprite = availableSprites[Random.Range(0, availableSprites.Length - 1)];
    }
}
