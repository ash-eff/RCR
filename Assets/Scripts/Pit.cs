using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Pit : MonoBehaviour
{
    private PlayerManager player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerPitTrigger"))
        {
            player.PlayerIsFalling(transform.position);
        }
    }
}
