using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;

public class Hazard : MonoBehaviour
{
    [SerializeField] private float hazardSize = 1;
    public string hazardDialogue = "You were hurt by a hazard!";
    public string typeOfHazard;


    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        boxCollider.size = new Vector3(hazardSize, hazardSize);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(hazardSize, hazardSize));
    }
}
