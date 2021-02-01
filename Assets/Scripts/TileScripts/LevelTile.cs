using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelTile : MonoBehaviour
{
    public Vector2 gridPos;
    public LevelGenerator levelGenerator;
    public SpriteRenderer bottomSprite;
    public Vector2 SetGridPos
    {
        set => gridPos = value;
    }

    private void Awake()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
    }

    private void Start()
    {
        AssignTileFeatures();
    }

    public void AssignTileFeatures()
    {
        CheckNeighbors();
        AssignSprite();
    }

    protected virtual void CheckNeighbors()
    {
    }

    protected virtual void AssignSprite()
    {
    }
}
