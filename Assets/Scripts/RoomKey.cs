using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomKey : MonoBehaviour
{
    private RoomManager roomManager;
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Sprite green;


    private void Awake()
    {
        roomManager = FindObjectOfType<RoomManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerRoomTrigger"))
        {
            spr.sprite = green;
            roomManager.CurrentRoom.UnlockDoors(true, true, true, true);
        }
    }
}
