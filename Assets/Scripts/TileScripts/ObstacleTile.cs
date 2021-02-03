using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTile : LevelTile
{
    protected override void CheckNeighbors()
    {
    }

    protected override void AssignSprite()
    {
        tileSprite.sprite = availableSprites[Random.Range(0, availableSprites.Length - 1)];
    }
}
