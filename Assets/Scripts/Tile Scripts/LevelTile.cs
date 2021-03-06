﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class LevelTile : MonoBehaviour
{
    public Vector2 gridPos;
    public Sprite[] availableSprites;
    public LevelGenerator levelGenerator;
    public SpriteRenderer tileSprite;
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
        SetColliderSize();
    }

    protected virtual void CheckNeighbors()
    {
    }

    protected virtual void AssignSprite()
    {
    }

    protected virtual void SetColliderSize()
    {
    }
}
