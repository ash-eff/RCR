using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using Unity.Mathematics;
using UnityEngine;
public class Projectile : MonoBehaviour
{
    [SerializeField] public float destroyAfter;
    [SerializeField] private GameObject impactPrefab;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SpriteRenderer spr;

    private Vector3 direction = Vector3.forward;
    public Vector3 SetDirection
    {
        set => direction = value;
    }

    public int damageAmount = 1;

    private void Start()
    {
        Destroy(gameObject, destroyAfter);  
        rb.velocity = new Vector3(transform.right.x, 0, transform.right.z) * speed;
        var spriteAngles = MyUtils.GetSpriteXYRotationFromYAngle(transform.rotation.eulerAngles.y);
        spr.transform.localRotation = Quaternion.Euler(spriteAngles.x, spriteAngles.y, spriteAngles.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 8 is the obstacle layer
        if (other.gameObject.layer == 8)
        {
            GameObject obj = Instantiate(impactPrefab, transform.position, quaternion.identity);
            Destroy(obj, .5f );
            Destroy(gameObject);
        }    
    }
}
