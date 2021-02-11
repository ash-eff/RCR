using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteAdjuster : MonoBehaviour
{
    private void Awake()
    {
        this.enabled = false;
    }

    private void Update()
    {
        transform.position = MyUtils.SpriteZAdjuster3D(transform.position);
    }
}
