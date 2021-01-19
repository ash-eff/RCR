using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerFallTrigger : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Collider2D fallTrigger;
    public bool inPit;
    private Vector3 currentPosition;
    private Vector3 lastPosition;
    public Vector3 jumpPosition;


    private void OnEnable()
    {
        inPit = false;
        fallTrigger.enabled = true;
    }

    private void Update()
    {
        currentPosition = transform.position;
    }

    private void LateUpdate()
    {
        if(!inPit)
            lastPosition = currentPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pit"))
        {
            fallTrigger.enabled = false;
            inPit = true;
            playerManager.PlayerIsFalling(lastPosition);
        }
    }
}
