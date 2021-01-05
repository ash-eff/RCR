﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public class Projectile : MonoBehaviour
{
    [SerializeField] public float destroyAfter;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Color A = Color.magenta;
    [SerializeField] private Color B = Color.blue;
    [SerializeField] private float lerpSpeed = 1.0f;

    public Weapon shotFromWeapon;
    public float delayTime;

    private void Start()
    {
        Destroy(gameObject, destroyAfter);  
        rb2d.velocity = transform.right * speed;
    }
    
    void Update()
    {
        spr.color = Color.Lerp(A, B, Mathf.PingPong(Time.time * lerpSpeed, 1.0f));
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.layer == 8)
    //    {
    //        Destroy(gameObject);
    //        GameObject obj = Instantiate(shotFromWeapon.impactPrefab, transform.position, quaternion.identity);
    //        Destroy(obj, .15f );
    //    }
//
    //    if (other.gameObject.layer == 11)
    //    {
    //        Destroy(gameObject);
    //        other.GetComponent<Dummy>().Hit(shotFromWeapon.damage);
    //        GameObject obj = Instantiate(shotFromWeapon.impactPrefab, other.transform.position, quaternion.identity);
    //        Destroy(obj, .15f );
    //    }
    //}


}
