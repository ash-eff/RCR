using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody rb;
    private SpriteRenderer spr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        Destroy(gameObject, 3f);  
        rb.velocity = new Vector3(transform.right.x, 0, transform.right.z) * 8f;
        var spriteAngles = MyUtils.GetSpriteXYRotationFromYAngle(transform.rotation.eulerAngles.y);
        spr.transform.localRotation = Quaternion.Euler(spriteAngles.x, spriteAngles.y, spriteAngles.z);
    }
}
