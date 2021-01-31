using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ammo : MonoBehaviour
{
    private int ammoAmount = 0;
    private PlayerManager player;
    
    private void Awake()
    {
        ammoAmount = Random.Range(3, 10);
        player = FindObjectOfType<PlayerManager>();
    }

    public int CollectAmmo()
    {
        Destroy(gameObject);
        return ammoAmount;
    }
}
