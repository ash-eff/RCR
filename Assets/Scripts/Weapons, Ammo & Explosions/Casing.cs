using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Casing : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float force;
    //[SerializeField] private Vector2 startPos;
    //[SerializeField] private Vector2 endPos;
    [SerializeField] private SpriteRenderer spr;
    //private bool canBounce = true;
    private float xOffset = 0;
    private float yOffset = 0;
    private Vector3 adjustedForce;
    
    void Start()
    {
        xOffset = Random.Range(-1f, 1f);
        yOffset = Random.Range(1, 2);
        //startPos = transform.position;
        adjustedForce = transform.up * force + new Vector3(xOffset, yOffset)  * force / 2;
        rigidbody.AddForce(adjustedForce);
        
        Destroy(gameObject, 15f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            rigidbody.useGravity = false;
        }
    }

    //void FixedUpdate()
    //{
    //    if (transform.position.y < startPos.y - yOffset && canBounce)
    //    {
    //        canBounce = false;
    //        rigidbody.gravityScale = 0;
    //        rigidbody.velocity = Vector3.zero;
    //    }
    //}
}
