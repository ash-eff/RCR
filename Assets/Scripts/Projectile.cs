using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] public float destroyAfter;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb2d;
    public Vector2 dir = Vector2.right;
    private void Start()
    {
        Destroy(gameObject, destroyAfter);
        
    }

    private void Update()
    {
        transform.Translate( (dir * (Time.deltaTime * speed)));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
