using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private float force;
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 endPos;
    [SerializeField] private SpriteRenderer spr;
    private bool canBounce = true;
    private float xOffset = 0;
    private float yOffset = 0;
    private Vector2 adjustedForce;
    
    void Start()
    {
        xOffset = Random.Range(-1f, 1f);
        yOffset = Random.Range(.5f, 1);
        startPos = transform.position;
        adjustedForce = transform.up * force + (new Vector3(xOffset, 0) * force / 2);
        rb2d.AddForce(adjustedForce);
        Destroy(gameObject, 15f);
    }
    
    void FixedUpdate()
    {
        if (transform.position.y < startPos.y - yOffset && canBounce)
        {
            canBounce = false;
            rb2d.gravityScale = 0;
            rb2d.velocity = Vector3.zero;
        }
    }
}
